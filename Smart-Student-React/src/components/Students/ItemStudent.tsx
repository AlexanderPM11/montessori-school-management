import img from "../../assets/images/student-placeholder.png";
import { useEffect, useState } from "react";
import { IoChevronDown, IoChevronUp } from "react-icons/io5";
import { Link } from "react-router-dom";
import { useGeneralStore } from "../../hooks/store/General/General.store";
import { formatReadableDate } from "../../util/formatReadableDate";
import { Student } from "../../interfaces/Students/Student";
import { useStudentStore } from "../../hooks/store/Student/Students.store";
import { SweetAlertCustom } from "../../util/SweetAlerCustom";
import { ToastifyCustom } from "../../util/ToastifyCustom ";

interface Props {
  student: Student;
  linkToEditOrCreate: string;
}

export const ItemStudent = ({ student, linkToEditOrCreate }: Props) => {
  const onGetData = useGeneralStore((state) => state.onGetData);

  const [isExpanded, setIsExpanded] = useState(false);

  const nationality = useGeneralStore((state) => state.nationality);

  const onInactiveActive = useStudentStore((state) => state.onInactiveActive);

  const setIscreating = useGeneralStore((state) => state.setIscreating);

  useEffect(() => {
    onGetData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const handleInactiveActive = (idStudent: number, estado: string) => {
    SweetAlertCustom({
      title: "¿Estás seguro?",
      description: `¿Deseas ${
        student.estado === "Activo" ? "inactivar" : "activar"
      } al Estudiante ${student.name ?? student.lastname}?`,
      type: "warning",
      showCancelButton: true,
      confirmButtonText: "Sí, continuar",
      cancelButtonText: "Cancelar",
      onConfirm: async () => {
        onInactiveActive(idStudent, estado).then((result) => {
          if (result.result) {
            ToastifyCustom({
              options: { autoClose: 2000, position: "bottom-right" },
              message: `El usuario fue ${
                student.estado === "Activo" ? "inactivado" : "activado"
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
              src={
                student.urlImg && student.urlImg !== "" ? student.urlImg : img
              }
              alt="User profile"
            />
            <div>
              <p className="text-sm font-bold truncate">
                {student.name ?? student.name} {student.lastname}
              </p>
              <p className="text-xs text-gray-600 truncate">
                {student.direction}
              </p>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <span
              className={`w-2 h-2 rounded-full ${
                student.estado === "Activo" ? "bg-green-500" : "bg-red-500"
              }`}
              title={student.estado ? "Activo" : "Inactivo"}
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
                <strong>Sexo:</strong> {student.sexDes || "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Grado:</strong> {student.gradeDes || "No disponible"}
              </p>

              <p className="text-xs text-gray-600">
                <strong>Nacionalidad:</strong>{" "}
                {nationality?.find(
                  (x) => x.id === Number(student.idNacionality)
                )?.text || "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Dirección:</strong>{" "}
                {student.direction || "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Fecha de Nacimiento:</strong>{" "}
                {student.bornDate
                  ? formatReadableDate(student.bornDate)
                  : "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Teléfono Padre:</strong> {student.telFather}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Teléfono Madre:</strong> {student.telMother}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Padre/Tutor:</strong> {student.idFatherDesc}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Madre/Tutor:</strong> {student.idMotherDesc}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Persona de Emergencia:</strong>{" "}
                {student.emergencyPerson || "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Teléfono de emergencia:</strong>{" "}
                {student.emergencyTel || "No disponible"}
              </p>
            </div>

            {/* Acciones */}
            <div className="flex gap-2 mt-3">
              <Link
                onClick={(e) => {
                  e.stopPropagation();
                  setIscreating(true);
                }}
                to={`${linkToEditOrCreate}?idStudent=${student.id}`}
                className="text-sm text-blue-600 hover:text-blue-800"
              >
                Editar
              </Link>
              <button
                className="text-sm text-red-600 hover:text-red-800"
                onClick={(e) => {
                  const estado =
                    student.estado === "Activo" ? "Inactivo" : "Activo";
                  e.stopPropagation(); // Evita que el acordeón se colapse
                  handleInactiveActive(student.id, estado);
                }}
              >
                {student.estado === "Activo" ? "Inactivar" : "Activar"}
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
