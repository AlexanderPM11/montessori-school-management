import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import "react-tabs/style/react-tabs.css";
import clsx from "clsx";
import { useState } from "react";

// Tipo de datos para cada pestaña
interface TabData {
  icon: JSX.Element;
  label: string;
  content: JSX.Element;
}

// Props del componente TabsComponent
interface TabsComponentProps {
  tabs: TabData[];
  activeTab?: number;
  onTabChange?: (index: number) => void;
}

export const TabsComponent: React.FC<TabsComponentProps> = ({
  tabs,
  activeTab,
  onTabChange,
}) => {
  // Estado interno si no se pasa desde props
  const [internalTabIndex, setInternalTabIndex] = useState(0);

  const handleTabChange = (index: number) => {
    if (onTabChange) {
      onTabChange(index);
    } else {
      setInternalTabIndex(index);
    }
  };

  const currentTab = activeTab ?? internalTabIndex;

  return (
    <div className="mx-auto w-full ">
      <Tabs selectedIndex={currentTab} onSelect={handleTabChange}>
        {/* Lista de pestañas */}
        <div className="overflow-x-auto custom-scrollbar">
          <TabList className="flex justify-start gap-4 border-b">
            {tabs.map((tab, index) => (
              <Tab
                key={index}
                className={clsx(
                  "flex flex-col items-center text-center px-4 py-2 focus:outline-none cursor-pointer",
                  "hover:text-gray-900",
                  "border-b-1 border-transparent",
                  "react-tabs__tab--selected:border-gray-900 react-tabs__tab--selected:text-gray-900"
                )}
              >
                <div className="flex items-center gap-2">
                  {tab.icon}
                  <span>{tab.label}</span>
                </div>
              </Tab>
            ))}
          </TabList>
        </div>

        {/* Paneles de contenido */}
        {tabs.map((tab, index) => (
          <TabPanel key={index}>{tab.content}</TabPanel>
        ))}
      </Tabs>
    </div>
  );
};
