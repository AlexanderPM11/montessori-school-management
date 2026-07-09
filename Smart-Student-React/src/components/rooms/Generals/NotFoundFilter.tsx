import { FiSearch } from "react-icons/fi";

export const NotFoundFilter = () => {
  return (
    <div className="flex-1 flex items-center justify-center">
      <div className="flex flex-col items-center justify-center space-y-3 p-12">
        <FiSearch className="text-gray-400 text-3xl" />
        <p className="text-gray-500 font-medium">No se encontraron usuarios</p>
        <p className="text-gray-400 text-sm">
          Intenta ajustar tus filtros de búsqueda
        </p>
      </div>
    </div>
  );
};
