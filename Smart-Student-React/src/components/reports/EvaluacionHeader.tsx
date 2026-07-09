import { Link } from "react-router-dom";
import { FiArrowLeft } from "react-icons/fi";

export const EvaluacionHeader = ({ idRoom }: { idRoom: number }) => (
  <Link
    className="border border-gray-900 h-10 w-10 rounded-full flex items-center justify-center text-[14px] 
    transition-all duration-200 ease-in-out bg-gray-900 text-white hover:bg-white hover:text-gray-900"
    to={`/rooms/students?idRoom=${idRoom}`}
    title="Volver"
  >
    <FiArrowLeft />
  </Link>
);
