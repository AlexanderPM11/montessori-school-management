import { useEffect } from "react";
import { CustomPagination } from "../../CustomPagination";
import { FaPlus } from "react-icons/fa";
import { Link, useSearchParams } from "react-router-dom";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../util/showCustomLoading";
import { useStudentRoomStore } from "../../../hooks/store/Room/StudentRoom";
import { SearchBar } from "../../Users/SearchBar";
import { ItemStudentRoom } from "./ItemStudentRoom";
import { FiArrowLeft } from "react-icons/fi";

export const StudentRoom = () => {
  const [searchParams] = useSearchParams();
  const idRoom = Number(searchParams.get("idRoom"));

  // Funciones del store
  const GetStudentsRoom = useStudentRoomStore((state) => state.GetStudentsRoom);
  const setCurrentDisplayStudents = useStudentRoomStore(
    (state) => state.setCurrentDisplayStudents
  );
  const setSearchTerm = useStudentRoomStore((state) => state.setSearchTermRoom);
  const getFilteredStudentsRoom = useStudentRoomStore(
    (state) => state.getFilteredStudentsRoom
  );
  // Variables del store
  const currentDisplayStudents = useStudentRoomStore(
    (state) => state.currentDisplayStudents
  );
  const students = useStudentRoomStore((state) => state.Students);
  const isLoading = useStudentRoomStore((state) => state.loading);
  const changesstudents = useStudentRoomStore((state) => state.changesStudents);
  const currentPage = useStudentRoomStore((state) => state.currentPage);
  const searchTerm = useStudentRoomStore((state) => state.searchTerm);

  const fetchData = async () => {
    try {
      await GetStudentsRoom(idRoom);
      getFilteredStudentsRoom();
    } catch (error) {
      console.error("Error fetching students:", error);
    }
  };

  useEffect(() => {
    fetchData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  const displayContennt = !isLoading && students && students.length > 0;
  if (isLoading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }

  return (
    <div className={`h-full ${isLoading ? "h-screen" : "mt-5"}`}>
      {/* Sección de Búsqueda */}{" "}
      {!isLoading && (
        <SearchBar
          searchTerm={searchTerm}
          onSearchChange={(value) => {
            setSearchTerm(value);
            getFilteredStudentsRoom();
          }}
        />
      )}
      <div className="flex justify-between mb-4 mt-4 gap-2 items-center">
        {displayContennt && (
          <div className="">
            <p className="font-semibold">Estudiantes agregados</p>
          </div>
        )}

        {/* Botón de crear */}
        <div className="flex gap-2">
          <Link
            className="bg-gray-900 border border-gray-900 h-10 w-10 text-white rounded-full 
                   flex items-center justify-center text-[14px] 
                   transition-all duration-200 ease-in-out 
                   hover:bg-white hover:text-gray-900"
            to={`/rooms/add-students?idRoom=${idRoom}`}
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
          currentDisplayStudents.map((user) => (
            <div key={user.id} className="w-full md:w-[calc(50%-0.5rem)]">
              <ItemStudentRoom
                linkToEditOrCreate="/students/edit-create"
                student={user}
                isAdding={false}
                idRoom={idRoom}
              />
            </div>
          ))}
      </div>
      {students && currentDisplayStudents.length <= 0 && (
        <div className="flex justify-center mt-5">
          <p className="font-semibold">
            No se encontraron estudiantes para este salón. Presione el botón "+"
            para agregar nuevos.
          </p>
        </div>
      )}
      {displayContennt && (
        <div className="flex justify-center mt-5 mb-5">
          <CustomPagination
            itemDisplayCount={10}
            items={changesstudents}
            pageCount={changesstudents.length / 10}
            currentPage={currentPage}
            onPageChange={(value, page) => {
              setCurrentDisplayStudents(value, page);
            }}
          />
        </div>
      )}
    </div>
  );
};
