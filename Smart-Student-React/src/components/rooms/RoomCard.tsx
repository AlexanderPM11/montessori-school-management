import { FC, useState } from "react";
import { Room } from "../../interfaces/Room/Room";
import { FaLayerGroup } from "react-icons/fa";
import img from "../../assets/images/placeholder-user.jpg";
import imgPlaceholder from "../../assets/images/placeholder.jpg";
import { FiTrash2 } from "react-icons/fi";
import { CiEdit } from "react-icons/ci";
import { Link, useNavigate } from "react-router-dom";
import { useRoomsStore } from "../../hooks/store/Room/Rooms.store";
import { FloatingMenu } from "../FloatingMenu";

import { handleDeleteRoom } from "./handleDeleteRoom";
import { getRoomOptions } from "./getRoomOptions";

interface RoomCardProps {
  room: Room;
}

export const RoomCard: FC<RoomCardProps> = ({ room }) => {
  const onDelete = useRoomsStore((state) => state.onDelete);
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const navigate = useNavigate();

  const options = getRoomOptions({ ...room, id: String(room.id) }, navigate);
  return (
    <div className="border border-gray-400 bg-white rounded-lg flex flex-col justify-between leading-normal shadow-md hover:shadow-lg transition-all max-w-[350px] w-full relative">
      <img
        src={room.imageUrl ?? imgPlaceholder}
        alt={room.name}
        className="w-full h-48 object-cover mb-3 rounded-t-lg"
      />
      <div className="pt-2 px-4">
        <p className="text-sm text-gray-600 flex items-center">
          <FaLayerGroup className="mr-2" />
          {room.level}
        </p>
        <h2 className="text-gray-900 font-bold text-lg mb-2 hover:text-gray-700">
          {room.name.length > 45
            ? room.name.substring(0, 45) + "..."
            : room.name}
        </h2>
        <p className="text-gray-700 text-sm mb-4">
          {room.description.length > 100
            ? room.description.substring(0, 100) + "..."
            : room.description}
        </p>
        <div className="flex items-center mb-2">
          <img
            className="w-10 h-10 rounded-full mr-4"
            src={img}
            alt={`Avatar of ${room.teacherFullName}`}
          />
          <div className="text-sm">
            <p className="text-gray-900 font-semibold leading-none hover:text-gray-700">
              {room.teacherFullName}
            </p>
            <p className="text-gray-600 text-[11px] font-bold">
              Maestro Asignado
            </p>
          </div>
        </div>

        {/* Botones de acción */}
        <div className="border-t mt-2 pt-2 flex justify-between items-center relative">
          <div className="flex gap-2">
            <Link
              to={`/rooms/edit-create?idRoom=${room.id}`}
              className="p-2 text-gray-500 hover:text-yellow-600 rounded-lg transition-colors flex items-center gap-1"
              title="Editar"
            >
              <CiEdit size={16} />
              <span className="text-xs hidden sm:inline">Editar</span>
            </Link>
            <button
              onClick={() => handleDeleteRoom(room.id, room.name, onDelete)}
              className="p-2 text-gray-500 hover:text-red-600 rounded-lg transition-colors flex items-center gap-1"
              title="Eliminar"
            >
              <FiTrash2 size={16} />
              <span className="text-xs hidden sm:inline">Eliminar</span>
            </button>
          </div>

          {/* Botón + para dropdown */}
          <div className="relative">
            <button
              onClick={() => setDropdownOpen((prev) => !prev)}
              className="p-2 text-gray-500 hover:text-gray-900rounded-lg transition-colors flex items-center justify-center"
              title="Más opciones"
              type="button"
            >
              <span className="text-xl font-bold select-none">+</span>
            </button>

            <FloatingMenu
              options={options}
              isOpen={dropdownOpen}
              onClose={() => setDropdownOpen(false)}
            />
          </div>
        </div>
      </div>
    </div>
  );
};
