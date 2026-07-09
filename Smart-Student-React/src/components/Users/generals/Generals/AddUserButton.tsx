import { FC } from "react";
import { Link } from "react-router-dom";
import { FaPlus } from "react-icons/fa";

interface AddUserButtonProps {
  onClick?: () => void;
  to?: string;
}

export const AddUserButton: FC<AddUserButtonProps> = ({
  onClick,
  to = "edit-create",
}) => {
  return (
    <Link
      to={to}
      onClick={onClick}
      className="bg-gray-900 border border-gray-900 h-10 w-10 text-white rounded-full 
                 flex items-center justify-center text-[14px] 
                 transition-all duration-200 ease-in-out 
                 hover:bg-white hover:text-gray-900"
      title="Crear nuevo"
    >
      <FaPlus />
    </Link>
  );
};
