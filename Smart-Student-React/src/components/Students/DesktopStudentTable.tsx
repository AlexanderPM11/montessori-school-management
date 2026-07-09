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
  FiCheck,
  FiX,
  FiXCircle,
  FiCheckCircle,
  FiEdit3,
  FiChevronRight,
  FiChevronLeft,
} from "react-icons/fi";

import { IoCloudUploadOutline } from "react-icons/io5";
import { Student } from "../../interfaces/Students/Student";
import { ApiResponse } from "../../interfaces/ApiResponse";
import ManageLocalStorage from "../../util/manageLocalStorage";
import { ToastifyCustom, useGeneralStore } from "../../Formik/Users/user";
import { formatReadableDate } from "../../util/formatReadableDate";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";
import { AddUserButton } from "./Generals/AddUserButton";
import { UploadFile } from "../UploadFile";
import { TableDisplay } from "./Generals/TableDisplay";
import { NotFoundData } from "./Generals/NoData";
import { NotFoundFilter } from "./Generals/NotFoundFilter";
import { Pagination } from "./Generals/Pagination";
import { handleGeneralFunctionStudent } from "../../util/Student/HandleGeneralFunctionStudent";
import { useStudentStore } from "../../hooks/store/Student/Students.store";
import img from "../../assets/images/placeholder-user.jpg";

interface DesktopUserParentsTableProps {
  users: Student[];
  onToggleStatus?: (id: number, estado: string) => Promise<ApiResponse<string>>;
  isParentsView?: boolean;
}

export const DesktopStudentTable: FC<DesktopUserParentsTableProps> = ({
  users,
  onToggleStatus,
  isParentsView = false,
}) => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [showExcelModal, setShowExcelModal] = useState(false);

  const LOCAL_STORAGE_KEY = "students-table-page-size";
  const defaultPageSize =
    ManageLocalStorage.readFromLocalStorage<number>(LOCAL_STORAGE_KEY) || 5;

  const civilStatus = useGeneralStore((state) => state.civilStatus);
  const nationality = useGeneralStore((state) => state.nationality);
  const professions = useGeneralStore((state) => state.professions);

  const [sorting, setSorting] = useState<SortingState>([]);
  const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]);
  const [globalFilter, setGlobalFilter] = useState<string>("");

  const columns = useMemo<ColumnDef<Student>[]>(
    () => [
      {
        header: "Foto",
        id: "photo",
        size: 60,
        cell: ({ row }) => {
          const image = row.original.urlImg;
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
        header: "Nombre completo",
        accessorFn: (row) => `${row.name} ${row.lastname ?? ""}`,
        size: 260,
        cell: ({ getValue }) => (
          <span className="font-medium text-gray-900">
            {getValue() as string}
          </span>
        ),
      },
      {
        header: "Nacimiento",
        accessorKey: "dateBirth",
        cell: ({ row }) => (
          <span className="text-gray-600">
            {formatReadableDate(row.original.bornDate)}
          </span>
        ),
        size: 170,
        enableColumnFilter: false,
      },
      {
        header: "Sexo",
        accessorKey: "sexDes",
        cell: ({ row }) => (
          <span className="text-xs text-gray-700">
            {row.original.sexDes || "No disponible"}
          </span>
        ),
        size: 100,
        enableColumnFilter: false,
      },
      {
        header: "Grado",
        accessorKey: "gradeDes",
        cell: ({ row }) => (
          <span className="text-xs text-gray-700">
            {row.original.gradeDes || "No disponible"}
          </span>
        ),
        size: 140,
      },
      {
        header: "Nacionalidad",
        id: "nacionalidad",
        cell: ({ row }) => {
          const nationalityText = nationality?.find(
            (x) => x.id === Number(row.original.idNacionality)
          )?.text;
          return (
            <span className="text-xs text-gray-700">
              {nationalityText || "No disponible"}
            </span>
          );
        },
        size: 140,
      },
      {
        header: "Dirección",
        accessorKey: "direction",
        cell: ({ row }) => (
          <span className="text-xs text-gray-700">
            {row.original.direction || "No disponible"}
          </span>
        ),
        size: 200,
      },
      {
        header: "Tel. Padre",
        accessorKey: "telFather",
        cell: ({ row }) => (
          <span className="text-xs text-gray-700">
            {row.original.telFather || "—"}
          </span>
        ),
        size: 120,
      },
      {
        header: "Tel. Madre",
        accessorKey: "telMother",
        cell: ({ row }) => (
          <span className="text-xs text-gray-700">
            {row.original.telMother || "—"}
          </span>
        ),
        size: 120,
      },
      {
        header: "Padre/Tutor",
        accessorKey: "idFatherDesc",
        cell: ({ row }) => (
          <span className="text-xs text-gray-700">
            {row.original.idFatherDesc || "—"}
          </span>
        ),
        size: 150,
      },
      {
        header: "Madre/Tutor",
        accessorKey: "idMotherDesc",
        cell: ({ row }) => (
          <span className="text-xs text-gray-700">
            {row.original.idMotherDesc || "—"}
          </span>
        ),
        size: 150,
      },
      {
        header: "Persona Emergencia",
        accessorKey: "emergencyPerson",
        cell: ({ row }) => (
          <span className="text-xs text-gray-700">
            {row.original.emergencyPerson || "—"}
          </span>
        ),
        size: 180,
      },
      {
        header: "Tel. Emergencia",
        accessorKey: "emergencyTel",
        cell: ({ row }) => (
          <span className="text-xs text-gray-700">
            {row.original.emergencyTel || "—"}
          </span>
        ),
        size: 150,
      },
      {
        header: "Estado",
        accessorKey: "estado",
        cell: ({ row }) => {
          const isActive = row.original.estado === "Activo";
          return (
            <span
              className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-medium ${
                isActive
                  ? "bg-green-50 text-green-700 border border-green-100"
                  : "bg-red-50 text-red-700 border border-red-100"
              }`}
            >
              {isActive ? (
                <>
                  <FiCheck className="mr-1" /> Activo
                </>
              ) : (
                <>
                  <FiX className="mr-1" /> Inactivo
                </>
              )}
            </span>
          );
        },
        enableColumnFilter: false,
        size: 110,
      },
      {
        header: "Acciones",
        id: "actions",
        cell: ({ row }) => (
          <div className="flex gap-2 items-center">
            <Link
              to={`/student/edit-create-student?idStudent=${row.original.id}&isParentsView=${isParentsView}`}
              className="text-blue-600 hover:text-blue-800 p-2 rounded-md hover:bg-blue-50 transition-colors flex items-center justify-center"
              title="Editar usuario"
            >
              <FiEdit3 size={16} />
            </Link>

            <button
              onClick={async () => {
                showCustomLoading();
                const result = await onToggleStatus?.(
                  row.original.id,
                  row.original.estado === "Activo" ? "Inactivado" : "Activo"
                );
                if (result?.result) {
                  ToastifyCustom({
                    message: `Usuario ${
                      row.original.estado !== "Inactivado"
                        ? "Inactivado"
                        : "Activado"
                    }: ${row.original.name} ${row.original.lastname}`,
                    type: "success",
                  });
                } else {
                  ToastifyCustom({
                    message: "Error al cambiar estado",
                    type: "error",
                  });
                }
                closeCustomLoading();
              }}
              className={`p-2 rounded-md transition-colors flex items-center justify-center ${
                row.original.estado
                  ? "text-red-600 hover:text-red-800 hover:bg-red-50"
                  : "text-green-600 hover:text-green-800 hover:bg-green-50"
              }`}
              title={
                row.original.estado ? "Inactivar usuario" : "Activar usuario"
              }
            >
              {row.original.estado ? (
                <FiXCircle size={16} />
              ) : (
                <FiCheckCircle size={16} />
              )}
            </button>
          </div>
        ),
        size: 100,
      },
    ],
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [onToggleStatus, professions, civilStatus, nationality]
  );

  const table = useReactTable<Student>({
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
  const setIscreating = useGeneralStore((state) => state.setIscreating);
  const getAllUsersAsync = useStudentStore((state) => state.getAllUsersAsync);
  const onImportUsers = useStudentStore((state) => state.onImportUsers);

  const fetchData = async () => {
    try {
      await getAllUsersAsync();
    } catch (error) {
      console.error("Error fetching users:", error);
    }
  };
  const handleUpload = async (
    file: File,
    onProgress: (progress: number, error?: string) => void
  ) => {
    try {
      handleGeneralFunctionStudent({
        file,
        onProgress,
        onImportUsers,
        localStorageKey: "uploadUserExcelStudent",
      });
    } catch (error) {
      console.error("Error uploading file:", error);
    }
  };

  return (
    <div className="space-y-6">
      {/* Botón de Agregar */}
      <div className="flex justify-between mb-4 mt-4 gap-2">
        {/* Botón de crear */}
        <AddUserButton
          to="/student/edit-create-student"
          onClick={() => setIscreating(false)}
        />

        <div>
          {showExcelModal && (
            <UploadFile
              file={selectedFile}
              onClose={() => {
                setShowExcelModal(false);
              }}
              onSuccessClick={() => {
                fetchData();
                setShowExcelModal(false);
              }}
              onUpload={handleUpload}
            />
          )}

          {/* Botón para subir archivo Excel (estilo outline) */}
          <label
            htmlFor="excel-upload"
            className="border border-gray-900 text-gray-900 px-4 h-10 flex items-center rounded-full cursor-pointer hover:bg-gray-900 hover:text-white transition-colors text-sm"
            title="Subir archivo Excel"
            onClick={() => {
              setSelectedFile(null);
              setShowExcelModal(true);
            }}
          >
            Excel
            <IoCloudUploadOutline className="ml-1" />
          </label>
        </div>
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
              <TableDisplay
                table={table}
                isParentsView={isParentsView}
                toLink="/student/edit-create-student"
              />
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
