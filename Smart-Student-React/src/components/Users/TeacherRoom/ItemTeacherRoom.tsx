import { IoChevronDown, IoChevronUp } from "react-icons/io5";
import {
  ToastifyCustom,
  useEffect,
  useGeneralStore,
  User,
  useState,
} from "../../../Formik/Users/user";
import img from "../../../assets/images/placeholder-user.jpg";
import { formatReadableDate } from "../../../util/formatReadableDate";
import { IoMdAdd, IoMdRemove } from "react-icons/io";
import { useTeacherRoomStore } from "../../../hooks/store/Users/TeacherRoom.store";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../util/showCustomLoading";

interface Props {
  user: User;
  linkToEditOrCreate: string;
  isAdding: boolean;
  idRoom: number;
}

export const ItemTeacherRoom = ({ user, isAdding, idRoom }: Props) => {
  const onCreateEdit = useGeneralStore((state) => state.onGetData);
  const onAddTeacherRoom = useTeacherRoomStore((state) => state.AddTeacherRoom);
  const QuitTeacherRoom = useTeacherRoomStore((state) => state.QuitTeacherRoom);

  const civilStatus = useGeneralStore((state) => state.civilStatus);
  const nationality = useGeneralStore((state) => state.nationality);
  const professions = useGeneralStore((state) => state.professions);

  const [isExpanded, setIsExpanded] = useState(false);

  useEffect(() => {
    onCreateEdit();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const handleDelete = async () => {
    showCustomLoading();
    const result = await QuitTeacherRoom({
      idRoom,
      idTeachers: [user.id],
    });
    closeCustomLoading();
    if (result.result) {
      ToastifyCustom({
        options: { autoClose: 2000 },
        message: result.message ?? "Datos actualizados exitosamente",
        type: "success",
      });
    } else {
      ToastifyCustom({
        message: result.message ?? "An error occurred",
        type: "error",
      });
    }
  };
  const handleAdd = async () => {
    showCustomLoading();
    const result = await onAddTeacherRoom({
      idRoom,
      idTeachers: [user.id],
    });
    closeCustomLoading();
    if (result.result) {
      ToastifyCustom({
        options: { autoClose: 2000 },
        message: result.message ?? "Datos actualizados exitosamente",
        type: "success",
      });
    } else {
      ToastifyCustom({
        message: result.message ?? "An error occurred",
        type: "error",
      });
    }
  };

  return (
    <div className="max-w-full md:max-w-[600px]">
      <div className="bg-white border border-gray-300 rounded-lg overflow-hidden">
        {/* Encabezado de la tarjeta */}
        <div
          className="flex items-center justify-between p-3 cursor-pointer"
          onClick={() => setIsExpanded(!isExpanded)}
        >
          <div className="flex items-center gap-2">
            <img
              className="w-10 h-10 rounded-full object-cover"
              src={user.urlImage && user.urlImage !== "" ? user.urlImage : img}
              alt="User profile"
            />
            <div>
              <p className="text-sm font-bold truncate">
                {user.firstName ?? user.userName} {user.lastName}
              </p>
              <p className="text-xs text-gray-600 truncate">{user.email}</p>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <span
              className={`w-2 h-2 rounded-full ${
                user.estado ? "bg-green-500" : "bg-red-500"
              }`}
              title={user.estado ? "Activo" : "Inactivo"}
            ></span>
            {isExpanded ? (
              <IoChevronUp className="text-gray-600" />
            ) : (
              <IoChevronDown className="text-gray-600" />
            )}
          </div>
        </div>

        {/* Botón Agregar (solo si está colapsado) */}
        {!isExpanded && (
          <div className="pb-3 flex justify-end">
            <button
              className="text-gray-900 font-medium px-4 text-sm rounded hover:text-gray-600"
              onClick={isAdding ? handleAdd : handleDelete}
            >
              <span className="flex items-center gap-1">
                {isAdding ? "Agregar" : "Eliminar"}{" "}
                {isAdding ? (
                  <IoMdAdd className="text-gray-900" />
                ) : (
                  <IoMdRemove className="text-red-600" />
                )}
              </span>
            </button>
          </div>
        )}

        {/* Contenido expandible */}
        {isExpanded && (
          <div className="p-3 border-t border-gray-200">
            <div className="flex flex-col gap-2">
              <p className="text-xs text-gray-600">
                <strong>Teléfono:</strong> {user.tel || "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Celular:</strong> {user.phoneNumber || "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Estado:</strong> {user.estado ? "Activo" : "Inactivo"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Dirección:</strong> {user.addres || "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Fecha de Nacimiento:</strong>{" "}
                {user.dateBirth
                  ? formatReadableDate(user.dateBirth)
                  : "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Género:</strong>{" "}
                {user.gender === 1
                  ? "Masculino"
                  : user.gender === 0
                  ? "Femenino"
                  : "No especificado"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Identificación:</strong>{" "}
                {user.identificationId || "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Profesión:</strong>{" "}
                {professions?.find((x) => x.id === Number(user.profession))
                  ?.text || "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Ocupación:</strong>{" "}
                {professions?.find((x) => x.id === Number(user.job))?.text ||
                  "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Estado Civil:</strong>{" "}
                {civilStatus?.find((x) => x.id === Number(user.civilStatus))
                  ?.text || "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Nacionalidad:</strong>{" "}
                {nationality?.find((x) => x.id === Number(user.nationality))
                  ?.text || "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Roles:</strong>{" "}
                {user.roles?.join(", ") || "No disponible"}
              </p>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
