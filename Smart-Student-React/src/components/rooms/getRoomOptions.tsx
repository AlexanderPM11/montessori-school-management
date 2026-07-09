import { GiTeacher } from "react-icons/gi";
import { MdOutlineSubject } from "react-icons/md";
import { PiStudentFill } from "react-icons/pi";
import { BsClipboardCheck } from "react-icons/bs";
import { NavigateFunction } from "react-router-dom";

interface Room {
  id: string;
  // Puedes agregar más propiedades si las necesitas
}

export const getRoomOptions = (room: Room, navigate: NavigateFunction) => [
  {
    label: "Profesores",
    icon: <GiTeacher className="text-gray-900" />,
    onClick: () => navigate(`/rooms/teachers?idRoom=${room.id}`),
  },
  {
    label: "Asignaturas",
    icon: <MdOutlineSubject className="text-gray-900" />,
    onClick: () => navigate(`/rooms/subjects?idRoom=${room.id}`),
  },
  {
    label: "Estudiantes",
    icon: <PiStudentFill className="text-gray-900" />,
    onClick: () => navigate(`/rooms/students?idRoom=${room.id}`),
  },
  {
    label: "Pase de lista",
    icon: <BsClipboardCheck className="text-gray-900" />,
    onClick: () => navigate("/rooms/attendance?idRoom=" + room.id),
  },
  // {
  //   label: "Registro",
  //   icon: <MdAppRegistration className="text-gray-900" />,
  //   onClick: () => alert("Ver detalles de la sala"),
  // },
];
