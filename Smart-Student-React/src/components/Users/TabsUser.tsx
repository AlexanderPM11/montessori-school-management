import { useEffect, useState } from "react";
import { TabsComponent } from "../TabsComponent";
import { RiAdminFill } from "react-icons/ri";
import { MdFamilyRestroom } from "react-icons/md";
import ManageLocalStorage from "../../util/manageLocalStorage";
import { PiStudentBold } from "react-icons/pi";
import { StudentsDesplay } from "../Students/StudentsDesplay";
import { GeneralAdministractiveUsers } from "./generals/GeneralAdministractiveUsers";
import { GeneralParents } from "./generals/GeneralParents";

export const TabsUsers = () => {
  const [activeTab, setActiveTab] = useState<number>(() => {
    return (
      ManageLocalStorage.readFromLocalStorage<number>("activeTabUsers") ?? 0
    );
  });

  useEffect(() => {
    ManageLocalStorage.saveToLocalStorage("activeTabUsers", activeTab);
  }, [activeTab]);

  const tabs = [
    {
      icon: <RiAdminFill size={16} />,
      label: "Administrativos",
      content: <GeneralAdministractiveUsers />,
    },
    {
      icon: <MdFamilyRestroom size={17} />,
      label: "Padres/Tutores",
      content: <GeneralParents />,
    },
    {
      icon: <PiStudentBold size={16} />,
      label: "Estudiantes",
      content: <StudentsDesplay />,
    },
  ];

  return (
    <div>
      <TabsComponent
        tabs={tabs}
        activeTab={activeTab}
        onTabChange={setActiveTab}
      />
    </div>
  );
};
