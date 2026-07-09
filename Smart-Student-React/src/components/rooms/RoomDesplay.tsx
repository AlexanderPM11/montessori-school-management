import { useEffect } from "react";
import { useRoomsStore } from "../../hooks/store/Room/Rooms.store";
import { RoomCard } from "./RoomCard";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";
import { Title } from "../Title";
import { Link } from "react-router-dom";
import { FaPlus } from "react-icons/fa";
import { SearchBar } from "../Users/SearchBar";
import { SearchRooms } from "../../util/Room/SearchRooms";
import { CustomPagination } from "../CustomPagination";
import { itemsPerPage } from "./generals";

export const RoomDesplay = () => {
  const getAllRoomsAsync = useRoomsStore((state) => state.getAllRoomsAsync);
  const setSearchTerm = useRoomsStore((state) => state.setSearchTerm);
  const setPageCount = useRoomsStore((state) => state.setPageCount);
  const setcurrentDisplayRooms = useRoomsStore(
    (state) => state.setcurrentDisplayRooms
  );
  const setChangesRooms = useRoomsStore((state) => state.setChangesRooms);

  const rooms = useRoomsStore((state) => state.rooms);
  const loading = useRoomsStore((state) => state.loading);
  const searchTerm = useRoomsStore((state) => state.searchTerm);
  const currentPage = useRoomsStore((state) => state.currentPage);
  const changesRooms = useRoomsStore((state) => state.changesRooms);
  const currentDisplayRooms = useRoomsStore(
    (state) => state.currentDisplayRooms
  );

  const getFilteredStudents = (value: string) => {
    const filteredUsers = SearchRooms({ rooms, searchTerm: value });

    const currentPageIndex = currentPage;
    const currentPageStart = currentPageIndex * itemsPerPage;
    const currentPageEnd = (currentPageIndex + 1) * itemsPerPage;

    if (value.trim() === "") {
      setPageCount(Math.ceil(rooms.length / itemsPerPage));
      setcurrentDisplayRooms(
        rooms.slice(currentPageStart, currentPageEnd),
        currentPage
      );
      setChangesRooms(rooms);
    } else {
      setPageCount(Math.ceil(filteredUsers.length / itemsPerPage));
      setcurrentDisplayRooms(
        filteredUsers.slice(currentPageStart, currentPageEnd),
        currentPage
      );
      setChangesRooms(filteredUsers);
    }
  };

  useEffect(() => {
    getAllRoomsAsync().then(() => {
      if (rooms.length) {
        getFilteredStudents(searchTerm);
      }
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  if (loading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }
  return (
    <div className="">
      <Title>Gestión de Aulas</Title>
      {/* Sección de Búsqueda */}
      {!loading && (
        <SearchBar
          searchTerm={searchTerm}
          onSearchChange={(value) => {
            setSearchTerm(value);
            getFilteredStudents(value);
          }}
        />
      )}
      <div className="flex justify-end mb-4 mt-4 gap-2">
        <Link
          className="bg-gray-900 border border-gray-900 h-10 w-10 text-white rounded-full 
             flex items-center justify-center text-[14px] 
             transition-all duration-200 ease-in-out 
             hover:bg-white hover:text-gray-900"
          to={"/rooms/edit-create"}
          title="Crear"
        >
          <FaPlus />
        </Link>
      </div>
      {rooms && currentDisplayRooms.length <= 0 && (
        <div className="flex justify-center mt-5">
          <p className="font-semibold">Salón no encontrado</p>
        </div>
      )}

      {!loading && (
        <div className="flex flex-wrap justify-center md:justify-start w-full gap-4 mb-14 md:mb-10">
          {currentDisplayRooms.map((room) => (
            <RoomCard key={room.id} room={room} />
          ))}
        </div>
      )}

      {!loading && (
        <div className="flex justify-center mt-5 mb-5">
          <CustomPagination
            itemDisplayCount={itemsPerPage}
            items={changesRooms}
            pageCount={changesRooms.length / itemsPerPage}
            currentPage={currentPage}
            onPageChange={(value, page) => {
              setcurrentDisplayRooms(value, page);
            }}
          />
        </div>
      )}
    </div>
  );
};
