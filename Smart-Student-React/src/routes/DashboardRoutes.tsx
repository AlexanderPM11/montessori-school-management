import { RouteObject } from "react-router-dom";

// Pages
import { Home } from "../pages/Home";
import { Otras01 } from "../pages/Otras01";
import { Otras02 } from "../pages/Otras02";
import { EditCreateUser } from "../pages/EditCreateUser";
import { Students } from "../pages/EditCreateStudent";

// Dashboard
import { DashboardPage } from "../components/dashboard/DashboardPage";

// Users
import { TabsUserLogin } from "../components/Users/LoginUser/TabsUserLogin";
import { GeneralAdministractiveUsers } from "../components/Users/generals/GeneralAdministractiveUsers";
import { GeneralParents } from "../components/Users/generals/GeneralParents";
import { GeneralStudents } from "../components/Students/GeneralStudents";
import { TabsParents } from "../components/Users/generals/TabsParents";

// Rooms
import { GeneralRoom } from "../components/rooms/GeneralRoom";
import { EditCreateRoomForm } from "../Formik/Room/EditCreateRoomForm";
import { TeacherRoom } from "../components/Users/TeacherRoom/TeacherRoom";
import { TeacherRoomAddToRoom } from "../components/Users/TeacherRoom/TeacherRoomAddToRoom";
import { StudentRoom } from "../components/rooms/students/StudentRoom";
import { StudentRoomAddToRoom } from "../components/rooms/students/StudentRoomAddToRoom";
import { SubjectRoom } from "../components/rooms/roomSubject/SubjectRoom";
import { EditCreateroomSubject } from "../Formik/Room/RoomSubject/EditCreateRoomSubject";

// Subjects
import { CreateOrEditForm } from "../Formik/Subject/CreateOrEdit";
import { GeneralSubjects } from "../components/Subject/GeneralSubjects";

// Evaluations
import { Evaluacion } from "../components/reports/prekinder/Evaluation";
import { Evaluacion as EvaluacionPrimaria } from "../components/reports/primaria/Evaluation";
import { AttendaceDisplay } from "../components/rooms/Attendace/AttendaceDisplay";
import { RoleGuard } from "../components/RoleGuard";

export const DashboardRoutes: RouteObject[] = [
  { path: "/dashboard", element: <DashboardPage /> },
  {
    path: "/home",
    element: (
      <RoleGuard allowedRoles={["Admin", "Coordinador"]}>
        <Home />
      </RoleGuard>
    ),
  },

  // Users
  { path: "/user-login-edit", element: <TabsUserLogin /> },
  {
    path: "/administractive-users",
    element: (
      <RoleGuard allowedRoles={["Admin", "Coordinador"]}>
        <GeneralAdministractiveUsers />
      </RoleGuard>
    ),
  },
  {
    path: "/parents-users",
    element: (
      <RoleGuard allowedRoles={["Admin"]}>
        <GeneralParents />
      </RoleGuard>
    ),
  },
  {
    path: "/students",
    element: (
      <RoleGuard allowedRoles={["Admin", "Coordinador"]}>
        <GeneralStudents />
      </RoleGuard>
    ),
  },
  { path: "/users/edit-create", element: <EditCreateUser /> },
  { path: "/users/edit-create-parents", element: <TabsParents /> },
  { path: "/student/edit-create-student", element: <Students /> },

  // Rooms (ejemplo con roles si quieres)
  {
    path: "/rooms",
    element: (
      <RoleGuard allowedRoles={["Profesor", "Admin", "Coordinador"]}>
        <GeneralRoom />
      </RoleGuard>
    ),
  },
  { path: "/rooms/edit-create", element: <EditCreateRoomForm /> },
  { path: "/rooms/teachers", element: <TeacherRoom /> },
  { path: "/rooms/add-teachers", element: <TeacherRoomAddToRoom /> },
  { path: "/rooms/students", element: <StudentRoom /> },
  { path: "/rooms/add-students", element: <StudentRoomAddToRoom /> },
  { path: "/rooms/subjects", element: <SubjectRoom /> },
  {
    path: "/rooms/create-edit-room-subject",
    element: <EditCreateroomSubject />,
  },
  {
    path: "/rooms/attendance",
    element: <AttendaceDisplay />,
  },

  // Subjects
  { path: "/subjects", element: <GeneralSubjects /> },
  { path: "/subjects/edit-create", element: <CreateOrEditForm /> },

  // Evaluations
  { path: "/student/evaluacion", element: <Evaluacion /> },
  { path: "/student/evaluacion-primaria", element: <EvaluacionPrimaria /> },

  // Otras
  { path: "/otras01", element: <Otras01 /> },
  { path: "/otras02", element: <Otras02 /> },
];
