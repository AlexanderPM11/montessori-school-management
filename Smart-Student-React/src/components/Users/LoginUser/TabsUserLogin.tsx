import { RiAdminFill } from "react-icons/ri";
import { TabsComponent } from "../../TabsComponent";
import { EditUserLoginForm } from "../../../Formik/Users/UserLogin/EditCreateuserForm";
// import { GiPadlock } from "react-icons/gi";

export const TabsUserLogin = () => {
  const tabs = [
    {
      icon: <RiAdminFill size={16} />,
      label: "Usuario",
      content: <EditUserLoginForm />,
    },
    // {
    //   icon: <GiPadlock size={16} />,
    //   label: "Seguridad",
    //   content: <GiPadlock />,
    // },
  ];

  return (
    <div className="mt-20">
      <TabsComponent tabs={tabs} />
    </div>
  );
};
