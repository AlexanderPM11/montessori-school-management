import { ReactElement } from "react";
import { NavLink } from "react-router-dom";
import { FaChevronRight } from "react-icons/fa6";

interface Props {
  name: string;
  path: string;
  icon: ReactElement;
  onClick?: () => void;
}

export const MenuItem = ({ name, icon, path, onClick: logOut }: Props) => {
  return (
    <div className="w-[95%] md:w-[100%]">
      <NavLink
        onClick={logOut}
        to={path}
        className={({ isActive }) =>
          `h-[40px] max-w-[400px] pl-2 pr-2 rounded focus:outline-none items-center text-white flex justify-between ${
            isActive ? "bg-gray-700" : ""
          }`
        }
      >
        <div className="flex">
          {icon}
          <p className="text-base leading-4 ml-4">{name}</p>
        </div>

        <FaChevronRight />
      </NavLink>
    </div>
  );
};
