import { Link } from "react-router-dom";
import {
  ToastifyCustom,
  useEffect,
  useGeneralStore,
  useState,
} from "../../../Formik/Users/user";
import img from "../../../assets/images/placeholder-user.jpg";

import { RoomSubject } from "../../../interfaces/Room/RoomSubject";
import { CiEdit } from "react-icons/ci";
import { IoMdRemove } from "react-icons/io";
import { roomSubjectStore } from "../../../hooks/store/Room/RoomSubject";
import Swal from "sweetalert2";

interface Props {
  subjRoom: RoomSubject;
  linkToEditOrCreate: string;
  isAdding: boolean;
  idRoom: number;
}

export const ItemTeacherRoom = ({ subjRoom, linkToEditOrCreate }: Props) => {
  const onCreateEdit = useGeneralStore((state) => state.onGetData);
  const onDelete = roomSubjectStore((state) => state.onDelete);

  const [isExpanded, setIsExpanded] = useState(false);

  useEffect(() => {
    onCreateEdit();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const handleDelete = async () => {
    const { id, nameGrade } = subjRoom;

    const confirm = await Swal.fire({
      title: "¿Eliminar asignación de materia?",
      html: `Estás a punto de eliminar la materia asignada: <strong>${nameGrade}</strong><br/>`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Sí, eliminar",
      cancelButtonText: "Cancelar",
    });

    if (!confirm.isConfirmed) return;

    const confirmInput = await Swal.fire({
      title: "Confirmación final",
      html: `Para confirmar, escribe el <strong>nombre exacto de la materia</strong>: <br/>
             <strong>"${nameGrade}"</strong>`,
      input: "text",
      inputPlaceholder: "Nombre de la materia",
      showCancelButton: true,
      confirmButtonText: "Eliminar definitivamente",
      cancelButtonText: "Cancelar",
      inputValidator: (value) => {
        if (value.trim() !== nameGrade.trim()) {
          return "El nombre no coincide. Por favor, escribe exactamente el nombre de la materia.";
        }
        return null;
      },
    });

    if (confirmInput.isConfirmed) {
      try {
        const result = await onDelete(id);
        if (result.result) {
          ToastifyCustom({
            message: result.message || "Asignación eliminada con éxito.",
            type: "success",
            options: { autoClose: 2000, position: "bottom-right" },
          });
        } else {
          ToastifyCustom({
            message:
              result.message?.[0] ||
              "Ocurrió un error al eliminar la asignación.",
            type: "error",
          });
        }
      } catch (error) {
        console.error("Error al eliminar la asignación:", error);
        ToastifyCustom({
          message: "Error inesperado al eliminar la asignación.",
          type: "error",
        });
      }
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
              src={img}
              alt="User profile"
            />
            <div>
              <p className="text-sm font-bold truncate">
                {subjRoom.nameGrade ?? subjRoom.nameGrade}
              </p>
              <p className="text-xs text-gray-600 truncate">
                {subjRoom.nameTeacher}
              </p>
            </div>
          </div>
        </div>
        <div className="pb-3 flex justify-end">
          <Link
            className="text-gray-900 pl-4 text-sm rounded hover:text-gray-600"
            to={`${linkToEditOrCreate}?id-subject-room=${subjRoom.id}&isEdit=true`}
          >
            <span className="flex items-center gap-1">
              Editar
              <CiEdit className="text-gray-900" />
            </span>
          </Link>
          <button
            className="px-4 text-sm rounded hover:text-red-400 text-red-600"
            onClick={handleDelete}
          >
            <span className="flex items-center gap-1">
              Eliminar
              <IoMdRemove className="text-red-600" />
            </span>
          </button>
        </div>
      </div>
    </div>
  );
};
