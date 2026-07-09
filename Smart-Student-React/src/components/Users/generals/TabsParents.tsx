import { RiAdminFill } from "react-icons/ri";
import { FaUserFriends } from "react-icons/fa";
import { TabsComponent } from "../../TabsComponent";
import { EditCreateuserParentsForm } from "../../../Formik/Users/EditCreateuserParentsForm";
import { useSearchParams } from "react-router-dom";
import { GeneralStudents } from "../../Students/GeneralStudents";

export const TabsParents = () => {
  const [searchParams] = useSearchParams();
  const idUser = searchParams.get("idUser") ?? "";

  const tabs = [
    {
      icon: <FaUserFriends size={16} />,
      label: "Padre/Tutore",
      content: <EditCreateuserParentsForm />,
    },
    ...(idUser
      ? [
          {
            icon: <RiAdminFill size={16} />,
            label: "Estudiante",
            content: <GeneralStudents idParent={idUser} isParentsView={true} />,
          },
        ]
      : []),
  ];

  return (
    <div>
      <TabsComponent tabs={tabs} />
      <div className="lg:mb-14 mb-5"></div>
    </div>
  );
};
