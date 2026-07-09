import { FC } from "react";
import { Table } from "@tanstack/react-table";
import {
  FiChevronLeft,
  FiChevronRight,
  FiChevronsLeft,
  FiChevronsRight,
} from "react-icons/fi";
import ManageLocalStorage from "../../../util/manageLocalStorage";
import { Room } from "../../../interfaces/Room/Room";

interface Props {
  table: Table<Room>;
  users: Room[];
}

export const Pagination: FC<Props> = ({ table, users }) => {
  const LOCAL_STORAGE_KEY = "rooms-table-page-size";

  return (
    <div className="flex items-center justify-between px-5 py-3 border-t border-gray-100 bg-white">
      <div className="flex items-center space-x-2">
        <button
          className="p-1 rounded-md border border-gray-200 bg-white text-gray-500 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
          onClick={() => table.setPageIndex(0)}
          disabled={!table.getCanPreviousPage()}
        >
          <FiChevronsLeft className="h-4 w-4" />
        </button>
        <button
          className="p-1 rounded-md border border-gray-200 bg-white text-gray-500 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
          onClick={() => table.previousPage()}
          disabled={!table.getCanPreviousPage()}
        >
          <FiChevronLeft className="h-4 w-4" />
        </button>
        <span className="text-sm text-gray-700">
          Página{" "}
          <strong>
            {table.getState().pagination.pageIndex + 1} de{" "}
            {table.getPageCount()}
          </strong>
        </span>
        <button
          className="p-1 rounded-md border border-gray-200 bg-white text-gray-500 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
          onClick={() => table.nextPage()}
          disabled={!table.getCanNextPage()}
        >
          <FiChevronRight className="h-4 w-4" />
        </button>
        <button
          className="p-1 rounded-md border border-gray-200 bg-white text-gray-500 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
          onClick={() => table.setPageIndex(table.getPageCount() - 1)}
          disabled={!table.getCanNextPage()}
        >
          <FiChevronsRight className="h-4 w-4" />
        </button>
      </div>

      <div className="flex items-center space-x-2">
        <span className="text-sm text-gray-700">Mostrar:</span>
        <select
          className="border border-gray-200 rounded-md px-2 py-1 text-sm bg-white"
          value={table.getState().pagination.pageSize}
          onChange={(e) => {
            ManageLocalStorage.saveToLocalStorage(
              LOCAL_STORAGE_KEY,
              Number(e.target.value)
            );
            table.setPageSize(Number(e.target.value));
          }}
        >
          {[5, 10, 20, 30, 40, 50].map((pageSize) => (
            <option key={pageSize} value={pageSize}>
              {pageSize}
            </option>
          ))}
        </select>
        <span className="text-sm text-gray-700">
          Total: {users.length} usuarios
        </span>
      </div>
    </div>
  );
};
