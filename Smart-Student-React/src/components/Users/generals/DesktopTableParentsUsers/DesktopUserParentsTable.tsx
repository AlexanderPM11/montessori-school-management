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
import { User } from "../../../../interfaces/Auth/User";
import ManageLocalStorage from "../../../../util/manageLocalStorage";
import {
  ToastifyCustom,
  useGeneralStore,
  useUserParentsStore,
} from "../../../../Formik/Users/user";
import { NotFoundData } from "../Generals/NoData";
import { NotFoundFilter } from "../Generals/NotFoundFilter";
import { Pagination } from "../Generals/Pagination";
import { formatReadableDate } from "../../../../util/formatReadableDate";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../../util/showCustomLoading";
import { IoCloudUploadOutline } from "react-icons/io5";
import { UploadFile } from "../../../UploadFile";
import { roles } from "../../../../util/GeneralConst";
import { handleGeneralFunctionUser } from "../../../../util/User/HandleGeneralFunctionUser";
import { useUserGeneralStore } from "../../../../hooks/store/Users/General";
import { AddUserButton } from "../Generals/AddUserButton";
import { TableDisplay } from "../Generals/TableDisplay";
import { ApiResponse } from "../../../../interfaces/ApiResponse";
import img from "../../../../assets/images/placeholder-user.jpg";

interface DesktopUserParentsTableProps {
  users: User[];
  onToggleStatus?: (id: string) => Promise<ApiResponse<string>>;
}

export const DesktopUserParentsTable: FC<DesktopUserParentsTableProps> = ({
  users,
  onToggleStatus,
}) => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [showExcelModal, setShowExcelModal] = useState(false);

  const LOCAL_STORAGE_KEY = "user-table-page-parents-size";
  const defaultPageSize =
    ManageLocalStorage.readFromLocalStorage<number>(LOCAL_STORAGE_KEY) || 5;

  const civilStatus = useGeneralStore((state) => state.civilStatus);
  const nationality = useGeneralStore((state) => state.nationality);
  const professions = useGeneralStore((state) => state.professions);

  const [sorting, setSorting] = useState<SortingState>([]);
  const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]);
  const [globalFilter, setGlobalFilter] = useState<string>("");

  const columns = useMemo<ColumnDef<User>[]>(
    () => [
      {
        header: "Foto",
        id: "photo",
        size: 60,
        cell: ({ row }) => {
          const image = row.original.urlImage;
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
        accessorFn: (row) =>
          `${row.firstName ?? row.userName} ${row.lastName ?? ""}`,
        size: 260,
        cell: ({ getValue }) => (
          <span className="font-medium text-gray-900">
            {getValue() as string}
          </span>
        ),
      },
      {
        header: "Correo",
        accessorKey: "email",
        size: 220,
        cell: ({ getValue }) => (
          <span className="text-gray-600">{getValue() as string}</span>
        ),
      },
      {
        header: "Contacto",
        id: "contact",
        size: 140,
        cell: ({ row }) => (
          <div className="flex flex-col">
            <span className="text-gray-700">{row.original.phoneNumber}</span>
            {row.original.tel && (
              <span className="text-xs text-gray-500">
                Tel: {row.original.tel}
              </span>
            )}
          </div>
        ),
        enableColumnFilter: false,
      },
      {
        header: "Nacimiento",
        accessorKey: "dateBirth",
        cell: ({ getValue }) => (
          <span className="text-gray-600">
            {formatReadableDate(getValue() as string)}
          </span>
        ),
        size: 120,
        enableColumnFilter: false,
      },
      {
        header: "Género",
        accessorKey: "gender",
        cell: ({ getValue }) => {
          const gender = getValue() as number;
          return (
            <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-gray-100 text-gray-800">
              {gender === 1
                ? "Masculino"
                : gender === 0
                ? "Femenino"
                : "No especificado"}
            </span>
          );
        },
        enableColumnFilter: false,
        size: 120,
      },
      {
        header: "Identificación",
        accessorKey: "identificationId",
        size: 140,
        cell: ({ getValue }) => (
          <span className="font-mono text-gray-700">
            {getValue() as string}
          </span>
        ),
      },
      {
        header: "Profesión",
        accessorKey: "profession",
        cell: ({ getValue }) => {
          const id = Number(getValue());
          const profession =
            professions?.find((x) => x.id === id)?.text ?? "No disponible";
          return <span className="text-gray-700">{profession}</span>;
        },
        size: 160,
        enableColumnFilter: false,
      },
      {
        header: "Roles",
        accessorKey: "roles",
        cell: ({ getValue }) => {
          const roles = getValue() as string[];
          return (
            <div className="flex flex-wrap gap-1">
              {roles?.map((role, i) => (
                <span
                  key={i}
                  className="bg-blue-50 text-blue-700 text-xs px-2 py-1 rounded-full border border-blue-100"
                >
                  {role}
                </span>
              ))}
            </div>
          );
        },
        enableColumnFilter: false,
        size: 230,
      },
      {
        header: "Estado",
        accessorKey: "estado",
        cell: ({ getValue }) => {
          const isActive = getValue() as boolean;
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
              to={`/users/edit-create-parents?idUser=${row.original.id}`}
              className="text-blue-600 hover:text-blue-800 p-2 rounded-md hover:bg-blue-50 transition-colors flex items-center justify-center"
              title="Editar usuario"
            >
              <FiEdit3 size={16} />
            </Link>

            <button
              onClick={async () => {
                showCustomLoading();
                const result = await onToggleStatus?.(row.original.id);
                if (result?.result) {
                  ToastifyCustom({
                    message: `Usuario ${
                      row.original.estado ? "inactivado" : "activado"
                    }: ${row.original.firstName} ${row.original.lastName}`,
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

  const table = useReactTable<User>({
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
  const getAllUsersAsync = useUserParentsStore(
    (state) => state.getAllUsersAsync
  );
  const onImportUsers = useUserGeneralStore((state) => state.onImportUsers);

  const rolesAdmin = [
    roles.admin,
    roles.coordinator,
    roles.teacher,
    roles.rector,
    roles.secretary,
    roles.basic,
  ];
  const fetchData = async (loadForce?: boolean) => {
    try {
      await getAllUsersAsync(rolesAdmin, loadForce);
    } catch (error) {
      console.error("Error fetching users:", error);
    }
  };
  const handleUpload = async (
    file: File,
    onProgress: (progress: number, error?: string) => void
  ) => {
    try {
      handleGeneralFunctionUser({
        file,
        onProgress,
        onImportUsers,
        localStorageKey: "uploadUserExcelAdministrati",
      });
    } catch (error) {
      console.error("Error uploading file:", error);
    }
  };
  return (
    <div className="pace-y-6">
      {/* Botón de Agregar */}
      <div className="flex justify-between mb-4 mt-4 gap-2">
        {/* Botón de crear */}
        <AddUserButton
          to="/users/edit-create-parents"
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
                fetchData(true);
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
              <TableDisplay table={table} toLink="/users/edit-create-parents" />
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
