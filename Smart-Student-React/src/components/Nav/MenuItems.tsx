// import { AiOutlineDashboard } from "react-icons/ai";
import { FiHome, FiBookOpen } from "react-icons/fi";
import { GiGraduateCap } from "react-icons/gi";
import { HiOutlineOfficeBuilding } from "react-icons/hi";
import { MdAdminPanelSettings } from "react-icons/md";
import { PiUsersThreeDuotone } from "react-icons/pi";

export const appMenuItems = [
  // {
  //   section: "main",
  //   path: "/dashboard",
  //   name: "Dashboard",
  //   icon: <AiOutlineDashboard className="w-5 h-5" />, // icono actualizado
  //   allowedRoles: ["Admin", "Coordinador"],
  // },
  {
    section: "main",
    path: "/home",
    name: "Centro",
    icon: <FiHome className="w-5 h-5" />,
    allowedRoles: ["Admin", "Coordinador"], // Solo Admin y Coordinador
  },
  // Academic users
  {
    section: "users",
    path: "/administractive-users",
    name: "Administrativos",
    icon: <MdAdminPanelSettings className="w-5 h-5" />,
    allowedRoles: ["Admin", "Coordinador"], // Solo Admin y Coordinador
  },
  {
    section: "users",
    path: "/parents-users",
    name: "Padres/Tutores",
    icon: <PiUsersThreeDuotone className="w-5 h-5" />,
    allowedRoles: ["Admin"], // Solo Admin
  },
  {
    section: "users",
    path: "/students",
    name: "Estudiantes",
    icon: <GiGraduateCap className="w-5 h-5" />,
    allowedRoles: ["Admin", "Coordinador"], // Admin y Coordinador
  },
  // Academic Section
  {
    section: "academic",
    path: "/subjects",
    name: "Asignaturas",
    icon: <FiBookOpen className="w-5 h-5" />,
    allowedRoles: ["Admin", "Coordinador"], // Admin y Coordinador
  },
  {
    section: "academic",
    path: "/rooms",
    name: "Salones",
    icon: <HiOutlineOfficeBuilding className="w-5 h-5" />,
    allowedRoles: ["Profesor", "Admin", "Coordinador"], // Profesor, Admin y Coordinador
  },
];
