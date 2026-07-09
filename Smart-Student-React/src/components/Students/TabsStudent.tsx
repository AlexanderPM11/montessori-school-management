import { TabsComponent } from "../TabsComponent";
import { RiAdminFill } from "react-icons/ri";
import { EditCreateStudentForm } from "../../Formik/Students/EditCreateStudentForm";
import { IoDocuments } from "react-icons/io5";
import { Documents } from "./Document/Documents";
import { useSearchParams } from "react-router-dom";

export const TabsStudent = () => {
  const [searchParams] = useSearchParams();

  const idStudent = Number(searchParams.get("idStudent"));

  const tabs = [
    {
      icon: <RiAdminFill size={16} />,
      label: "Estudiante",
      content: <EditCreateStudentForm />,
    },
    {
      icon: <IoDocuments size={16} />,
      label: "Documentos",
      content: <Documents studentId={idStudent} />,
    },
  ];

  return (
    <div className="">
      <TabsComponent tabs={tabs} />
    </div>
  );
};
