// components/SearchBar.tsx
import { IoFilter } from "react-icons/io5";
import { FiX } from "react-icons/fi";
import { SearchForm } from "../../Formik/Search/SearchForm";

interface SearchBarProps {
  searchTerm: string;
  onSearchChange: (value: string) => void;
  onClearSearch?: () => void;
}

export const SearchBar: React.FC<SearchBarProps> = ({
  searchTerm,
  onSearchChange,
}) => {
  return (
    <div className="bg-white rounded-lg p-3 shadow-sm border border-gray-200 w-full">
      <div className="flex flex-wrap items-center justify-between mb-3 gap-2">
        <div className="flex items-center gap-2">
          <IoFilter className="text-indigo-600 text-lg" />
          <h3 className="text-base font-medium text-gray-800">Filtros</h3>
        </div>
        {searchTerm && (
          <button
            onClick={() => {
              searchTerm = "";
              onSearchChange(searchTerm);
            }}
            className="flex items-center text-sm text-gray-500 hover:text-gray-700 transition"
          >
            <FiX className="mr-1" />
            Limpiar
          </button>
        )}
      </div>
      <SearchForm search={searchTerm} onChangeCustom={onSearchChange} />
    </div>
  );
};
