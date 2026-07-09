import { useEffect, useState } from "react";
import { useUserParentsStore } from "../../../hooks/store/Users/Parents.store";
import { searchUser } from "../../../util/User/SearchUser";
import { ItemUser } from "../ItemUser";
import { CustomPagination } from "../../CustomPagination";
import { SearchBar } from "../SearchBar";
import { FaPlus } from "react-icons/fa";
import { Link } from "react-router-dom";
import { useGeneralStore } from "../../../hooks/store/General/General.store";
import { IoCloudUploadOutline } from "react-icons/io5";
import { UploadFile } from "../../UploadFile";
import { useUserGeneralStore } from "../../../hooks/store/Users/General";
import { roles } from "../../../util/GeneralConst";
import { handleGeneralFunctionUser } from "../../../util/User/HandleGeneralFunctionUser";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../util/showCustomLoading";

export const Parents = () => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [showExcelModal, setShowExcelModal] = useState(false);

  // Funciones del store
  const getAllUsersAsync = useUserParentsStore(
    (state) => state.getAllUsersAsync
  );
  const onImportUsers = useUserGeneralStore((state) => state.onImportUsers);
  const setCurrentDisplayUsers = useUserParentsStore(
    (state) => state.setCurrentDisplayUsers
  );
  const onInactiveActive = useUserParentsStore(
    (state) => state.onInactiveActive
  );
  const setChangesUsers = useUserParentsStore((state) => state.setChangesUsers);
  const setPageCount = useUserParentsStore((state) => state.setPageCount);
  const setSearchTerm = useUserParentsStore((state) => state.setSearchTerm);
  const setIscreating = useGeneralStore((state) => state.setIscreating);

  // Variables del store
  const currentDisplayUsers = useUserParentsStore(
    (state) => state.currentDisplayUsers
  );
  const users = useUserParentsStore((state) => state.users);
  const isLoading = useUserParentsStore((state) => state.loading);
  const changesUsers = useUserParentsStore((state) => state.changesUsers);
  const currentPage = useUserParentsStore((state) => state.currentPage);
  const searchTerm = useUserParentsStore((state) => state.searchTerm);

  const getFilteredUsers = (value: string) => {
    const filteredUsers = searchUser({ users, searchTerm: value });

    const currentPageIndex = currentPage;
    const currentPageStart = currentPageIndex * 10;
    const currentPageEnd = (currentPageIndex + 1) * 10;

    if (value.trim() === "") {
      setPageCount(Math.ceil(users.length / 10));
      setCurrentDisplayUsers(
        users.slice(currentPageStart, currentPageEnd),
        currentPage
      );
      setChangesUsers(users);
    } else {
      setPageCount(Math.ceil(filteredUsers.length / 10));
      setCurrentDisplayUsers(
        filteredUsers.slice(currentPageStart, currentPageEnd),
        currentPage
      );
      setChangesUsers(filteredUsers);
    }
  };
  const rolesParents = [roles.parents];

  const fetchData = async (loadForce?: boolean) => {
    try {
      await getAllUsersAsync(rolesParents, loadForce);
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
        localStorageKey: "uploadUserExcelParents",
      });
    } catch (error) {
      console.error("Error uploading file:", error);
    }
  };

  useEffect(() => {
    if (!users.length) {
      fetchData();
    } else {
      getFilteredUsers(searchTerm);
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [JSON.stringify(roles)]);

  const displayContennt = !isLoading && users && users.length > 0;

  if (isLoading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }

  return (
    <div className={`h-full ${isLoading ? "h-screen" : "mt-5"}`}>
      {/* Sección de Búsqueda */}
      {displayContennt && (
        <SearchBar
          searchTerm={searchTerm}
          onSearchChange={(value) => {
            setSearchTerm(value);
            getFilteredUsers(value);
          }}
        />
      )}

      {/* Botón de Agregar */}
      <div className="flex justify-end mb-4 mt-4 gap-2">
        {/* Botón de crear */}
        <Link
          className="bg-gray-900 border border-gray-900 h-10 w-10 text-white rounded-full 
             flex items-center justify-center text-[14px] 
             transition-all duration-200 ease-in-out 
             hover:bg-white hover:text-gray-900"
          to={"edit-create-parents"}
          onClick={() => setIscreating(false)}
          title="Crear nuevo"
        >
          <FaPlus />
        </Link>

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

      <div className="flex flex-wrap justify-start gap-4 items-start ">
        {displayContennt &&
          currentDisplayUsers.map((user) => (
            <div key={user.id} className="w-full md:w-[calc(50%-0.5rem)]">
              <ItemUser
                onInactiveActive={onInactiveActive}
                user={user}
                linkToEditOrCreate="/users/edit-create-parents"
              />
            </div>
          ))}
      </div>

      {users && currentDisplayUsers.length <= 0 && (
        <div className="flex justify-center mt-5">
          <p className="font-semibold">Usuarios no encontrados</p>
        </div>
      )}

      {displayContennt && (
        <div className="flex justify-center mt-5 mb-5">
          <CustomPagination
            itemDisplayCount={10}
            items={changesUsers}
            pageCount={changesUsers.length / 10}
            currentPage={currentPage}
            onPageChange={(value, page) => {
              setCurrentDisplayUsers(value, page);
            }}
          />
        </div>
      )}
    </div>
  );
};
