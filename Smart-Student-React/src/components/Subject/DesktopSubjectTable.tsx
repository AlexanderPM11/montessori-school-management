import { FC, useMemo, useRef, useState } from "react";
import {
  useReactTable,
  getCoreRowModel,
  ColumnDef,
  getFilteredRowModel,
  getSortedRowModel,
  SortingState,
  ColumnFiltersState,
  getPaginationRowModel,
} from "@tanstack/react-table";
import { Link } from "react-router-dom";
import {
  FiEdit3,
  FiChevronRight,
  FiChevronLeft,
  FiTrash,
} from "react-icons/fi";

import { ApiResponse } from "../../interfaces/ApiResponse";
import ManageLocalStorage from "../../util/manageLocalStorage";
import { useGeneralStore } from "../../Formik/Users/user";
import { TableDisplay } from "./Generals/TableDisplay";
import { NotFoundData } from "./Generals/NoData";
import { NotFoundFilter } from "./Generals/NotFoundFilter";
import { Pagination } from "./Generals/Pagination";
import img from "../../assets/images/placeholder-user.jpg";
import { Subject } from "../../interfaces/Subject/Subject";
import { handleInactiveActive } from "./handleInactiveActive";
import { FaPlus } from "react-icons/fa";

interface DesktopUserParentsTableProps {
  users: Subject[];
  onDelete: (idRoom: number) => Promise<ApiResponse<number>>;
}

export const DesktopSubjectTable: FC<DesktopUserParentsTableProps> = ({
  users,
  onDelete,
}) => {
  const LOCAL_STORAGE_KEY = "students-table-page-size";
  const defaultPageSize =
    ManageLocalStorage.readFromLocalStorage<number>(LOCAL_STORAGE_KEY) || 5;

  const civilStatus = useGeneralStore((state) => state.civilStatus);
  const nationality = useGeneralStore((state) => state.nationality);
  const professions = useGeneralStore((state) => state.professions);

  const [sorting, setSorting] = useState<SortingState>([]);
  const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]);
  const [globalFilter, setGlobalFilter] = useState<string>("");

  const columns = useMemo<ColumnDef<Subject>[]>(
    () => [
      {
        header: "Foto",
        id: "photo",
        size: 60,
        cell: ({ row }) => {
          const image = row.original.imageUrl;
          const src = image?.startsWith("data:image")
            ? image
            : image
            ? `data:image/jpeg;base64,${image}`
            : img;
          return (
            <img
              src={src}
              alt="Foto de perfil"
              className="w-10 h-10 rounded-full object-cover border border-gray-200"
            />
          );
        },
        enableColumnFilter: false,
      },
      {
        header: "Nombre",
        accessorFn: (row) => `${row.name}`,
        size: 260,
        cell: ({ getValue }) => (
          <span className="font-medium text-gray-900">
            {getValue() as string}
          </span>
        ),
      },
      {
        header: "Descripción",
        accessorFn: (row) => `${row.name}`,
        size: 260,
        cell: ({ row }) => (
          <span className="font-medium text-gray-900">
            {row.original.description}
          </span>
        ),
      },
      {
        header: "Código",
        accessorKey: "code",
        cell: ({ row }) => (
          <span className="font-medium text-gray-900">{row.original.code}</span>
        ),
        size: 120,
      },

      {
        header: "Acciones",
        id: "actions",
        cell: ({ row }) => (
          <div className="flex gap-2 items-center">
            <Link
              to={`/subjects/edit-create?idSubject=${row.original.id}`}
              className="text-blue-600 hover:text-blue-800 p-2 rounded-md hover:bg-blue-50 transition-colors flex items-center justify-center"
              title="Editar usuario"
            >
              <FiEdit3 size={16} />
            </Link>

            <button
              onClick={async () => {
                handleInactiveActive(row.original.id, onDelete, row.original);
              }}
              className={`p-2 rounded-md transition-colors flex items-center justify-center text-red-600 hover:text-red-800 hover:bg-red-50`}
              title={"Eliminar Asignatura"}
            >
              <FiTrash size={16} />
            </button>
          </div>
        ),
        size: 100,
      },
    ],
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [onDelete, professions, civilStatus, nationality]
  );

  const table = useReactTable<Subject>({
    data: users,
    columns,
    initialState: {
      pagination: {
        pageSize: defaultPageSize,
      },
    },
    state: {
      sorting,
      columnFilters,
      globalFilter,
    },
    onSortingChange: setSorting,
    onColumnFiltersChange: setColumnFilters,
    onGlobalFilterChange: setGlobalFilter,
    getCoreRowModel: getCoreRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
  });

  const scrollRef = useRef<HTMLDivElement>(null);

  const scrollLeft = () => {
    scrollRef.current?.scrollBy({ left: -200, behavior: "smooth" });
  };

  const scrollRight = () => {
    scrollRef.current?.scrollBy({ left: 200, behavior: "smooth" });
  };

  return (
    <div className="space-y-6">
      {/* Botón de Agregar */}
      <div className="flex justify-end mb-4 mt-4 gap-2">
        {/* Botón de crear */}
        <Link
          className="bg-gray-900 border border-gray-900 h-10 w-10 text-white rounded-full 
                   flex items-center justify-center text-[14px] 
                   transition-all duration-200 ease-in-out 
                   hover:bg-white hover:text-gray-900"
          to={"/subjects/edit-create"}
          title="Crear"
        >
          <FaPlus />
        </Link>
      </div>

      <div className="relative">
        {/* Flechas Scroll Horizontales */}
        <button
          onClick={scrollLeft}
          className="absolute left-3 top-1/2 -translate-y-1/2 z-10 bg-white border rounded-full p-2 shadow hover:bg-gray-100"
        >
          <FiChevronLeft />
        </button>

        <button
          onClick={scrollRight}
          className="absolute right-3 top-1/2 -translate-y-1/2 z-10 bg-white border rounded-full p-2 shadow hover:bg-gray-100"
        >
          <FiChevronRight />
        </button>

        {/* Tabla */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
          <div ref={scrollRef} className="overflow-x-auto">
            <div className="h-[600px] flex flex-col">
              <TableDisplay table={table} toLink="/subjects/edit-create" />
              {users.length === 0 && <NotFoundFilter />}
              {table.getRowModel().rows.length === 0 && users.length > 0 && (
                <NotFoundData />
              )}
            </div>
          </div>

          {/* Paginación */}
          <Pagination table={table} users={users} />
        </div>
      </div>
    </div>
  );
};
