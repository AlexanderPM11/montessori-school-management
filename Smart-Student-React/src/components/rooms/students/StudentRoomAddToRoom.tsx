import { useEffect } from "react";
import { CustomPagination } from "../../CustomPagination";
import { useNavigate, useSearchParams } from "react-router-dom";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../util/showCustomLoading";
import { FiArrowLeft } from "react-icons/fi";
import { ItemStudentRoom } from "./ItemStudentRoom";
import { useStudentRoomStore } from "../../../hooks/store/Room/StudentRoom";
import { SearchBar } from "../../Users/SearchBar";

export const StudentRoomAddToRoom = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const idRoom = Number(searchParams.get("idRoom"));

  // Funciones del store
  const GetStudentsToAddRoom = useStudentRoomStore(
    (state) => state.GetStudentsToAddRoom
  );
  const setCurrentDisplayStudentsToAddRoom = useStudentRoomStore(
    (state) => state.setCurrentDisplayStudentsToAddRoom
  );
  const setSearchTerm = useStudentRoomStore(
    (state) => state.setSearchTermToAddRoom
  );

  const getFilteredStudentsToAddRoom = useStudentRoomStore(
    (state) => state.getFilteredStudentsToAddRoom
  );

  // Variables del store
  const currentDisplayStudentsToAddRoom = useStudentRoomStore(
    (state) => state.currentDisplayUsersToAddRoom
  );
  const StudentsToAddRoom = useStudentRoomStore(
    (state) => state.StudentsToAddRoom
  );
  const isLoading = useStudentRoomStore((state) => state.loading);
  const changesStudentsToAddRoom = useStudentRoomStore(
    (state) => state.changesStudentsToAddRoom
  );
  const currentPage = useStudentRoomStore(
    (state) => state.currentPageToAddRoom
  );
  const searchTerm = useStudentRoomStore((state) => state.searchTermToAddRoom);

  const fetchData = async () => {
    try {
      await GetStudentsToAddRoom(idRoom);
      getFilteredStudentsToAddRoom();
    } catch (error) {
      console.error("Error fetching StudentsToAddRoom:", error);
    }
  };

  useEffect(() => {
    fetchData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  const displayContennt =
    !isLoading && StudentsToAddRoom && StudentsToAddRoom.length > 0;

  if (isLoading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }

  return (
    <div className={`h-full ${isLoading ? "h-screen" : "mt-5"}`}>
      {/* Sección de Búsqueda */}

      {!isLoading && (
        <SearchBar
          searchTerm={searchTerm}
          onSearchChange={(value) => {
            setSearchTerm(value);
            getFilteredStudentsToAddRoom();
          }}
        />
      )}
      <div className="flex justify-between mb-4 mt-4 gap-2 items-center">
        {displayContennt && (
          <div className="">
            <p className="font-semibold">Estudiantes sin agregar</p>
          </div>
        )}
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
          currentDisplayStudentsToAddRoom.map((user) => (
            <div key={user.id} className="w-full md:w-[calc(50%-0.5rem)]">
              <ItemStudentRoom
                linkToEditOrCreate="/StudentsToAddRoom/edit-create"
                student={user}
                isAdding={true}
                idRoom={idRoom}
              />
            </div>
          ))}
      </div>

      {StudentsToAddRoom && currentDisplayStudentsToAddRoom.length <= 0 && (
        <div className="flex justify-center ">
          <p className="font-semibold">No hay estudiantes disponibles</p>
        </div>
      )}

      {displayContennt && (
        <div className="flex justify-center mt-5 mb-5">
          <CustomPagination
            itemDisplayCount={10}
            items={changesStudentsToAddRoom}
            pageCount={changesStudentsToAddRoom.length / 10}
            currentPage={currentPage}
            onPageChange={(value, page) => {
              setCurrentDisplayStudentsToAddRoom(value, page);
            }}
          />
        </div>
      )}
    </div>
  );
};
