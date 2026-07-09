import { ReactElement, useState } from "react";
import { FaChevronDown, FaChevronUp } from "react-icons/fa";
import { NavLink } from "react-router-dom";

interface ItemMenu {
  icon: ReactElement;
  label: string;
  path: string;
}

interface Props {
  title: string;
  options: ItemMenu[];

  onclik?: () => void;
}

export const DropdownMenu = ({ title, options, onclik }: Props) => {
  const [isOpen, setIsOpen] = useState(false);

  const toggleMenu = () => {
    setIsOpen(!isOpen);
  };

  return (
    <div className="w-[95%] md:w-[100%]">
      <button
        onClick={toggleMenu}
        className="max-w-[400px] w-[100%] pl-2 pr-2 focus:outline-none text-left text-white flex items-center justify-between"
      >
        <span className="pr-5">{title}</span>
        {isOpen ? <FaChevronUp /> : <FaChevronDown />}
      </button>
      {isOpen && (
        <div className="flex justify-start flex-col w-full md:w-auto items-start pb-1">
          {options.map((option, index) => (
            <NavLink
              onClick={onclik}
              key={index}
              to={option.path}
              className="flex justify-start items-center space-x-6 hover:text-white focus:bg-gray-700 focus:text-white hover:bg-gray-700 text-gray-400 rounded px-3 py-2 w-full md:w-52"
            >
              {option.icon}
              <p className="text-base leading-4">{option.label}</p>
            </NavLink>
          ))}
        </div>
      )}
    </div>
  );
};
