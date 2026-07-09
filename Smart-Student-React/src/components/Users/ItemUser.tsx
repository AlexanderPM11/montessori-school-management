import { User } from "../../interfaces/Auth/User";
import img from "../../assets/images/placeholder-user.jpg";
import { ToastifyCustom } from "../../util/ToastifyCustom ";
import { SweetAlertCustom } from "../../util/SweetAlerCustom";
import { useEffect, useState } from "react";
import { IoChevronDown, IoChevronUp } from "react-icons/io5";
import { Link } from "react-router-dom";
import { useGeneralStore } from "../../hooks/store/General/General.store";
import { formatReadableDate } from "../../util/formatReadableDate";
import { ApiResponse } from "../../interfaces/ApiResponse";

interface Props {
  user: User;
  linkToEditOrCreate: string;
  onInactiveActive: (idUser: string) => Promise<ApiResponse<string>>;
}

export const ItemUser = ({
  user,
  linkToEditOrCreate,
  onInactiveActive,
}: Props) => {
  const onCreateEdit = useGeneralStore((state) => state.onGetData);

  const civilStatus = useGeneralStore((state) => state.civilStatus);
  const nationality = useGeneralStore((state) => state.nationality);
  const professions = useGeneralStore((state) => state.professions);

  const [isExpanded, setIsExpanded] = useState(false);

  const setIscreating = useGeneralStore((state) => state.setIscreating);

  const handleInactiveActive = (idUser: string) => {
    SweetAlertCustom({
      title: "¿Estás seguro?",
      description: `¿Deseas ${
        user.estado ? "inactivar" : "activar"
      } al usuario ${user.firstName ?? user.userName}?`,
      type: "warning",
      showCancelButton: true,
      confirmButtonText: "Sí, continuar",
      cancelButtonText: "Cancelar",
      onConfirm: async () => {
        onInactiveActive(idUser).then((result) => {
          if (result.result) {
            ToastifyCustom({
              options: { autoClose: 2000, position: "bottom-right" },
              message: `El usuario fue ${
                user.estado ? "inactivado" : "activado"
              } correctamente.`,
              type: "success",
            });
          } else {
            ToastifyCustom({
              message: result.message ? result.message[0] : "An error occurred",
              type: "error",
            });
          }
        });
      },
    });
  };
  useEffect(() => {
    onCreateEdit();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
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
            {/* Ícono de flecha */}
            {isExpanded ? (
              <IoChevronUp className="text-gray-600" />
            ) : (
              <IoChevronDown className="text-gray-600" />
            )}
          </div>
        </div>

        {/* Contenido expandible */}
        {isExpanded && (
          <div className="p-3 border-t border-gray-200">
            <div className="flex flex-col gap-2">
              {/* Información básica */}
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
              {/* Información profesional */}
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
              {/* Información adicional */}
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

            {/* Acciones */}
            <div className="flex gap-2 mt-3">
              <Link
                onClick={(e) => {
                  e.stopPropagation();
                  setIscreating(true);
                }}
                to={`${linkToEditOrCreate}?idUser=${user.id}`}
                className="text-sm text-blue-600 hover:text-blue-800"
              >
                Editar
              </Link>
              <button
                className="text-sm text-red-600 hover:text-red-800"
                onClick={(e) => {
                  e.stopPropagation(); // Evita que el acordeón se colapse
                  handleInactiveActive(user.id);
                }}
              >
                {user.estado ? "Inactivar" : "Activar"}
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
