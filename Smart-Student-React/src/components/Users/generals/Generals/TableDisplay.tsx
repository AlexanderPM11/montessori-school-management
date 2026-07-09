import { FC } from "react";
import { flexRender, Table } from "@tanstack/react-table";
import { FaSort, FaSortDown, FaSortUp } from "react-icons/fa";
import { User } from "../../../../interfaces/Auth/User";
import { useNavigate } from "react-router-dom";
import { Filter } from "./Filter";

interface Props {
  table: Table<User>;
  toLink: string;
}

export const TableDisplay: FC<Props> = ({ table, toLink }) => {
  const navigate = useNavigate();
  return (
    <table className="w-full">
      <thead className="bg-gray-50 border-b border-gray-100 sticky top-0 z-10">
        {table.getHeaderGroups().map((headerGroup) => (
          <tr key={headerGroup.id}>
            {headerGroup.headers.map((header) => (
              <th
                key={header.id}
                className="px-5 py-3.5 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider"
              >
                <div
                  className="flex flex-col space-y-2"
                  style={{ width: header.getSize() }}
                >
                  <div
                    className="flex items-center justify-between cursor-pointer group"
                    onClick={header.column.getToggleSortingHandler()}
                  >
                    <span className="flex-1">
                      {flexRender(
                        header.column.columnDef.header,
                        header.getContext()
                      )}
                    </span>
                    {header.column.getCanSort() && (
                      <span className="ml-2 transition-colors text-gray-300 group-hover:text-gray-400">
                        {{
                          asc: <FaSortUp className="text-blue-500" />,
                          desc: <FaSortDown className="text-blue-500" />,
                        }[header.column.getIsSorted() as string] ?? <FaSort />}
                      </span>
                    )}
                  </div>
                  {header.column.columnDef.enableColumnFilter !== false &&
                    header.column.getCanFilter() && (
                      <div className="mt-1">
                        <Filter column={header.column} />
                      </div>
                    )}
                </div>
              </th>
            ))}
          </tr>
        ))}
      </thead>
      <tbody className="divide-y divide-gray-100 overflow-y-auto">
        {table.getRowModel().rows.map((row) => (
          <tr
            onDoubleClick={() =>
              navigate(`${toLink}?idUser=${row.original.id}`)
            }
            key={row.id}
            className="hover:bg-gray-50 transition-colors cursor-pointer"
          >
            {row.getVisibleCells().map((cell) => (
              <td
                key={cell.id}
                className="px-5 py-3.5 text-sm"
                style={{ width: cell.column.getSize() }}
              >
                {flexRender(cell.column.columnDef.cell, cell.getContext())}
              </td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
};
