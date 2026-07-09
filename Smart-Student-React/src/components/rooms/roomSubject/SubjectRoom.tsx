import { useEffect } from "react";
import { CustomPagination } from "../../CustomPagination";
import { FaPlus } from "react-icons/fa";
import { Link, useSearchParams } from "react-router-dom";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../util/showCustomLoading";
import { ItemTeacherRoom } from "./ItemSubjectRoom";
import { roomSubjectStore } from "../../../hooks/store/Room/RoomSubject";
import { SearchBar } from "../../Users/SearchBar";
import { SearchSubjectRoom } from "../../../util/Room/SearchSubjectRoom";
import { FiArrowLeft } from "react-icons/fi";

export const SubjectRoom = () => {
  const [searchParams] = useSearchParams();
  const idRoom = Number(searchParams.get("idRoom"));

  // Funciones del store
  const GetAllRoomTeacher = roomSubjectStore((state) => state.GetAllRoomSubjet);
  const setCurrentDisplaysubjectRoom = roomSubjectStore(
    (state) => state.setcurrentDisplayRoomSubject
  );
  const setSearchTerm = roomSubjectStore((state) => state.setSearchTerm);
  const setChangessubjectRoom = roomSubjectStore(
    (state) => state.setChangesRoomSubject
  );
  const setPageCount = roomSubjectStore((state) => state.setPageCount);
  // Variables del store
  const currentDisplaysubjectRoom = roomSubjectStore(
    (state) => state.currentDisplayRoomSubjects
  );
  const subjectRoom = roomSubjectStore((state) => state.roomSubjet);
  const isLoading = roomSubjectStore((state) => state.loading);
  const changessubjectRoom = roomSubjectStore(
    (state) => state.changesRoomSubjects
  );
  const currentPage = roomSubjectStore((state) => state.currentPage);
  const searchTerm = roomSubjectStore((state) => state.searchTerm);

  const fetchData = async () => {
    try {
      await GetAllRoomTeacher(idRoom);
    } catch (error) {
      console.error("Error fetching subjectRoom:", error);
    }
  };

  const getFilteredsubjectRoom = (value: string) => {
    const filteredsubjectRoom = SearchSubjectRoom({
      roomSubject: subjectRoom,
      searchTerm: value,
    });

    const currentPageIndex = currentPage;
    const currentPageStart = currentPageIndex * 10;
    const currentPageEnd = (currentPageIndex + 1) * 10;

    if (value.trim() === "") {
      setPageCount(Math.ceil(subjectRoom.length / 10));
      setCurrentDisplaysubjectRoom(
        subjectRoom.slice(currentPageStart, currentPageEnd),
        currentPage
      );
      setChangessubjectRoom(subjectRoom);
    } else {
      setPageCount(Math.ceil(filteredsubjectRoom.length / 10));
      setCurrentDisplaysubjectRoom(
        filteredsubjectRoom.slice(currentPageStart, currentPageEnd),
        currentPage
      );
      setChangessubjectRoom(filteredsubjectRoom);
    }
  };

  useEffect(() => {
    fetchData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  const displayContennt = !isLoading && subjectRoom && subjectRoom.length > 0;

  if (isLoading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }

  return (
    <div className={`h-full ${isLoading ? "h-screen" : "mt-5"}`}>
      {/* Sección de Búsqueda */}
      {displayContennt && (
        <SearchBar
          searchTerm={searchTerm}
          onSearchChange={(value) => {
            setSearchTerm(value);
            getFilteredsubjectRoom(value);
          }}
        />
      )}

      {/* Botón de Agregar */}
      <div className="flex justify-between mb-4 mt-4 gap-2 items-center">
        {displayContennt && (
          <div className="">
            <p className="font-semibold">Listado de Asignaturas</p>
          </div>
        )}
        {/* Botón de crear */}
        <div className="flex gap-2">
          <Link
            className="bg-gray-900 border border-gray-900 h-10 w-10 text-white rounded-full 
             flex items-center justify-center text-[14px] 
             transition-all duration-200 ease-in-out 
             hover:bg-white hover:text-gray-900"
            to={`/rooms/create-edit-room-subject?idRoom=${idRoom}&isEdit=false`}
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
          currentDisplaysubjectRoom.map((subjRoom) => (
            <div key={subjRoom.id} className="w-full md:w-[calc(50%-0.5rem)]">
              <ItemTeacherRoom
                linkToEditOrCreate="/rooms/create-edit-room-subject"
                subjRoom={subjRoom}
                isAdding={false}
                idRoom={idRoom}
              />
            </div>
          ))}
      </div>

      {(!currentDisplaysubjectRoom ||
        (Array.isArray(currentDisplaysubjectRoom) &&
          currentDisplaysubjectRoom.length === 0) ||
        (typeof currentDisplaysubjectRoom === "object" &&
          !Array.isArray(currentDisplaysubjectRoom) &&
          Object.keys(currentDisplaysubjectRoom).length === 0)) && (
        <div className="flex justify-center mt-5">
          <p className="font-semibold">
            No hay Asignaturas disponibles para este salón. Presione el botón
            "+" para agregar una nueva asignatura.
          </p>
        </div>
      )}

      {displayContennt && (
        <div className="flex justify-center mt-5 mb-5">
          <CustomPagination
            itemDisplayCount={10}
            items={changessubjectRoom}
            pageCount={changessubjectRoom.length / 10}
            currentPage={currentPage}
            onPageChange={(value, page) => {
              setCurrentDisplaysubjectRoom(value, page);
            }}
          />
        </div>
      )}
    </div>
  );
};
