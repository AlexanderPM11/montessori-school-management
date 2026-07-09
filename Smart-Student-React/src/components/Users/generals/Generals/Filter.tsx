import { Column } from "@tanstack/react-table";
import { FC, useCallback } from "react";
import { FaFilter } from "react-icons/fa";
import { User } from "../../../../interfaces/Auth/User";
import { DebouncedInput } from "./DebouncedInput";

interface FilterProps {
  column: Column<User, unknown>;
}

export const Filter: FC<FilterProps> = ({ column }) => {
  const columnFilterValue = column.getFilterValue();

  const handleChange = useCallback(
    (value: string | number) => {
      column.setFilterValue(value);
    },
    [column]
  );

  return (
    <div className="relative">
      <div className="absolute inset-y-0 left-0 pl-2.5 flex items-center pointer-events-none">
        <FaFilter className="text-gray-300 text-xs" />
      </div>
      <DebouncedInput
        type="text"
        value={(columnFilterValue ?? "") as string}
        onChange={handleChange}
        placeholder="Filtrar..."
        className="block w-full pl-8 pr-2 py-1.5 border border-gray-200 rounded-md text-xs leading-4 bg-white placeholder-gray-400 focus:outline-none focus:ring-1 focus:ring-blue-100 focus:border-blue-400 transition-all"
        debounce={300}
      />
    </div>
  );
};
