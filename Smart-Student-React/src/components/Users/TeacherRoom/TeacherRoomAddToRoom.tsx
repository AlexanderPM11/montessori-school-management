import { useEffect } from "react";
import { CustomPagination } from "../../CustomPagination";
import { SearchBar } from "../SearchBar";
import { useNavigate, useSearchParams } from "react-router-dom";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../util/showCustomLoading";
import { useTeacherRoomStore } from "../../../hooks/store/Users/TeacherRoom.store";
import { ItemTeacherRoom } from "./ItemTeacherRoom";
import { searchUser } from "../../../util/User/SearchUser";
import { FiArrowLeft } from "react-icons/fi";
import { Title } from "../../Title";

export const TeacherRoomAddToRoom = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const idRoom = Number(searchParams.get("idRoom"));

  // Funciones del store
  const GetTeacherToAddRoom = useTeacherRoomStore(
    (state) => state.GetTeacherToAddRoom
  );
  const setCurrentDisplayUsers = useTeacherRoomStore(
    (state) => state.setCurrentDisplayUsersToAddRoom
  );
  const setSearchTerm = useTeacherRoomStore(
    (state) => state.setSearchTermToAddRoom
  );
  const setChangesUsers = useTeacherRoomStore(
    (state) => state.setChangesUsersToAddRoom
  );
  const setPageCount = useTeacherRoomStore(
    (state) => state.setPageCountToAddRoom
  );
  // Variables del store
  const currentDisplayUsers = useTeacherRoomStore(
    (state) => state.currentDisplayUsersToAddRoom
  );
  const users = useTeacherRoomStore((state) => state.teacherToAddRoom);
  const isLoading = useTeacherRoomStore((state) => state.loading);
  const changesUsers = useTeacherRoomStore(
    (state) => state.changesUsersToAddRoom
  );
  const currentPage = useTeacherRoomStore(
    (state) => state.currentPageToAddRoom
  );
  const searchTerm = useTeacherRoomStore((state) => state.searchTermToAddRoom);

  const fetchData = async () => {
    try {
      await GetTeacherToAddRoom(idRoom);
    } catch (error) {
      console.error("Error fetching users:", error);
    }
  };

  const getFilteredUsers = (value: string) => {
    const filteredUsers = searchUser({ users, searchTerm: value });

    const currentPageIndex = currentPage;
    const currentPageStart = currentPageIndex * 10;
    const currentPageEnd = (currentPageIndex + 1) * 10;

    if (value.trim() === "") {
      setPageCount(Math.ceil(users.length / 10));
      setCurrentDisplayUsers(
        users.slice(currentPageStart, currentPageEnd),
        currentPage
      );
      setChangesUsers(users);
    } else {
      setPageCount(Math.ceil(filteredUsers.length / 10));
      setCurrentDisplayUsers(
        filteredUsers.slice(currentPageStart, currentPageEnd),
        currentPage
      );
      setChangesUsers(filteredUsers);
    }
  };

  useEffect(() => {
    fetchData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  const displayContennt = !isLoading && users && users.length > 0;

  if (isLoading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }

  return (
    <div className={`h-full ${isLoading ? "h-screen" : "mt-5"}`}>
      <Title>Profesores sin agregar</Title>
      {/* Sección de Búsqueda */}
      {displayContennt && (
        <SearchBar
          searchTerm={searchTerm}
          onSearchChange={(value) => {
            setSearchTerm(value);
            getFilteredUsers(value);
          }}
        />
      )}

      <div className="flex justify-between mb-4 mt-4 gap-2 items-center">
        {/* Botón de crear */}
        {!isLoading && (
          <button
            type="button"
            onClick={() => navigate(-1)}
            className="bg-gray-900 border border-gray-900 h-10 w-10 text-white rounded-full 
              flex items-center justify-center text-[14px] 
              transition-all duration-200 ease-in-out 
              hover:bg-white hover:text-gray-900"
          >
            <FiArrowLeft className="text-[19px]" />
          </button>
        )}
      </div>

      <div className="flex flex-wrap justify-start gap-4 items-start mt-7">
        {displayContennt &&
          currentDisplayUsers.map((user) => (
            <div key={user.id} className="w-full md:w-[calc(50%-0.5rem)]">
              <ItemTeacherRoom
                linkToEditOrCreate="/users/edit-create"
                user={user}
                isAdding={true}
                idRoom={idRoom}
              />
            </div>
          ))}
      </div>

      {users && currentDisplayUsers.length <= 0 && (
        <div className="flex justify-center ">
          <p className="font-semibold">No hay profesores disponibles</p>
        </div>
      )}

      {displayContennt && (
        <div className="flex justify-center mt-5 mb-5">
          <CustomPagination
            itemDisplayCount={10}
            items={changesUsers}
            pageCount={changesUsers.length / 10}
            currentPage={currentPage}
            onPageChange={(value, page) => {
              setCurrentDisplayUsers(value, page);
            }}
          />
        </div>
      )}
    </div>
  );
};
