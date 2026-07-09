import img from "../../../assets/images/placeholder.jpg";
import { useState } from "react";
import { IoChevronDown, IoChevronUp } from "react-icons/io5";
import { ToastifyCustom, useGeneralStore } from "../../../Formik/Users/user";
import { Student } from "../../../interfaces/Students/Student";
import { formatReadableDate } from "../../../util/formatReadableDate";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../util/showCustomLoading";
import { useStudentRoomStore } from "../../../hooks/store/Room/StudentRoom";
import { FloatingMenu } from "../../FloatingMenu";
// import { HiOutlineDocumentReport } from "react-icons/hi";
import { BsFiletypePdf } from "react-icons/bs";

interface Props {
  student: Student;
  linkToEditOrCreate: string;
  isAdding: boolean;
  idRoom: number;
}

export const ItemStudentRoom = ({ student, isAdding, idRoom }: Props) => {
  const [isExpanded, setIsExpanded] = useState(false);
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const nationality = useGeneralStore((state) => state.nationality);

  const onAddStudentRoom = useStudentRoomStore((state) => state.AddStudentRoom);
  const onQuitStudentRoom = useStudentRoomStore(
    (state) => state.QuitStudentRoom
  );

  const handleDelete = async () => {
    showCustomLoading();
    const result = await onQuitStudentRoom(student.id);
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
    const result = await onAddStudentRoom({
      idRoom,
      idStudents: [student.id],
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

  // const queryParamsStudent = new URLSearchParams({
  //   idStudent: student.id.toString(),
  //   idRoom: idRoom.toString(),
  //   studentname: student.name ?? "",
  //   studentlastname: student.lastname ?? "",
  // });
  // const primaryGrades = ["Segundo", "Tercero", "Cuarto", "Quinto", "Sexto"];
  // const grade = student.gradeDes.trim();

  // const url = primaryGrades.includes(grade)
  //   ? `/student/evaluacion-primaria?${queryParamsStudent.toString()}`
  //   : `/student/evaluacion?${queryParamsStudent.toString()}`;

  const options = [
    // {
    //   label: "Evaluación",
    //   icon: <HiOutlineDocumentReport className="text-gray-900" />,
    //   onClick: () => navigate(url),
    // },
    {
      label: "Informe",
      icon: <BsFiletypePdf className="text-gray-900" />,
      onClick: () => {},
    },
  ];
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
        {/* Botón Agregar (solo si está colapsado) */}

        {!isExpanded && (
          <div className="pb-3 px-3 flex items-center justify-between text-xs text-gray-600">
            {!isAdding && (
              <p>
                <strong>Número:</strong> {student.numberList || "No disponible"}
              </p>
            )}
            <button
              className={`px-4 flex items-center gap-1 text-sm transition-colors ${
                isAdding
                  ? "text-green-600 hover:text-green-800"
                  : "text-red-600 hover:text-red-800"
              }`}
              onClick={isAdding ? handleAdd : handleDelete}
            >
              <span className="flex items-center gap-1">
                {isAdding ? "Agregar" : "Eliminar"}
              </span>
            </button>
            {/* Botón + para dropdown */}
            {!isAdding && (
              <div className="relative">
                <button
                  onClick={() => setDropdownOpen((prev) => !prev)}
                  className="p-2 text-gray-500 hover:text-gray-900 rounded-sm transition-colors flex items-center justify-center"
                  title="Más opciones"
                  type="button"
                >
                  <span className="text-xl font-bold select-none">+</span>
                </button>

                {dropdownOpen && (
                  <div className="absolute right-0 top-full z-50">
                    <FloatingMenu
                      options={options}
                      isOpen={dropdownOpen}
                      onClose={() => setDropdownOpen(false)}
                    />
                  </div>
                )}
              </div>
            )}
          </div>
        )}

        {/* Contenido expandible */}
        {isExpanded && (
          <div className="p-3 border-t border-gray-200">
            <div className="flex flex-col gap-2">
              {/* Información básica */}
              <p className="text-xs text-gray-600">
                <strong>Número:</strong> {student.numberList || "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Edad:</strong> {student.age} años
              </p>

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
              <button
                onClick={isAdding ? handleAdd : handleDelete}
                className={`text-sm flex items-center gap-1 transition-colors ${
                  isAdding
                    ? "text-green-600 hover:text-green-800"
                    : "text-red-600 hover:text-red-800"
                }`}
              >
                <span className="flex items-center gap-1">
                  {isAdding ? "Agregar" : "Eliminar"}
                </span>
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
