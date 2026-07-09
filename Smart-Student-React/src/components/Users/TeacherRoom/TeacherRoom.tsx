import { useEffect } from "react";
import { CustomPagination } from "../../CustomPagination";
import { SearchBar } from "../SearchBar";
import { FaPlus } from "react-icons/fa";
import { Link, useSearchParams } from "react-router-dom";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../util/showCustomLoading";
import { useTeacherRoomStore } from "../../../hooks/store/Users/TeacherRoom.store";
import { ItemTeacherRoom } from "./ItemTeacherRoom";
import { searchUser } from "../../../util/User/SearchUser";
import { FiArrowLeft } from "react-icons/fi";
import { Title } from "../../Title";

export const TeacherRoom = () => {
  const [searchParams] = useSearchParams();
  const idRoom = Number(searchParams.get("idRoom"));

  // Funciones del store
  const GetTeacherToAddRoom = useTeacherRoomStore(
    (state) => state.GetTeacherRoom
  );
  const setCurrentDisplayUsers = useTeacherRoomStore(
    (state) => state.setCurrentDisplayUsers
  );
  const setSearchTerm = useTeacherRoomStore((state) => state.setSearchTermRoom);
  const setChangesUsers = useTeacherRoomStore((state) => state.setChangesUsers);
  const setPageCount = useTeacherRoomStore((state) => state.setPageCountRoom);
  // Variables del store
  const currentDisplayUsers = useTeacherRoomStore(
    (state) => state.currentDisplayUsers
  );
  const users = useTeacherRoomStore((state) => state.teacher);
  const isLoading = useTeacherRoomStore((state) => state.loading);
  const changesUsers = useTeacherRoomStore((state) => state.changesUsers);
  const currentPage = useTeacherRoomStore((state) => state.currentPage);
  const searchTerm = useTeacherRoomStore((state) => state.searchTerm);

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
      <Title>Profesores agregados</Title>
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

      {/* Botón de Agregar */}
      <div className="flex justify-between mb-4 mt-4 gap-2 items-center">
        {/* Botón de crear */}
        <div className="flex gap-2">
          <Link
            className="bg-gray-900 border border-gray-900 h-10 w-10 text-white rounded-full 
             flex items-center justify-center text-[14px] 
             transition-all duration-200 ease-in-out 
             hover:bg-white hover:text-gray-900"
            to={`/rooms/add-teachers?idRoom=${idRoom}`}
            title="Crear nuevo"
          >
            <FaPlus />
          </Link>
          <Link
            className=" border border-gray-900 h-10 w-10 rounded-full 
                     flex items-center justify-center text-[14px] 
                     transition-all duration-200 ease-in-out 
                     hover:bg-gray-900 hover:text-white text-gray-900 "
            to={`/rooms/`}
            title="Volver"
          >
            <FiArrowLeft />
          </Link>
        </div>
      </div>

      <div className="flex flex-wrap justify-start gap-4 items-start ">
        {displayContennt &&
          currentDisplayUsers.map((user) => (
            <div key={user.id} className="w-full md:w-[calc(50%-0.5rem)]">
              <ItemTeacherRoom
                linkToEditOrCreate="/users/edit-create"
                user={user}
                isAdding={false}
                idRoom={idRoom}
              />
            </div>
          ))}
      </div>

      {users && currentDisplayUsers.length <= 0 && (
        <div className="flex justify-center mt-5">
          <p className="font-semibold">
            No se encontraron profesores para este salón. Presione el botón "+"
            para agregar nuevos docentes.
          </p>
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
