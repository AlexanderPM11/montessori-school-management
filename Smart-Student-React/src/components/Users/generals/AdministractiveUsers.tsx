import { useEffect, useState } from "react";
import { useUserStore } from "../../../hooks/store/Users/Users.store";
import { searchUser } from "../../../util/User/SearchUser";
import { ItemUser } from "../ItemUser";
import { CustomPagination } from "../../CustomPagination";
import { SearchBar } from "../SearchBar";
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
import { AddUserButton } from "./Generals/AddUserButton";

export const AdministractiveUsers = () => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [showExcelModal, setShowExcelModal] = useState(false);

  // Funciones del store
  const getAllUsersAsync = useUserStore((state) => state.getAllUsersAsync);
  const onInactiveActive = useUserStore((state) => state.onInactiveActive);
  const onImportUsers = useUserGeneralStore((state) => state.onImportUsers);
  const setCurrentDisplayUsers = useUserStore(
    (state) => state.setCurrentDisplayUsers
  );
  const setChangesUsers = useUserStore((state) => state.setChangesUsers);
  const setPageCount = useUserStore((state) => state.setPageCount);
  const setSearchTerm = useUserStore((state) => state.setSearchTerm);
  const setIscreating = useGeneralStore((state) => state.setIscreating);

  // Variables del store
  const currentDisplayUsers = useUserStore(
    (state) => state.currentDisplayUsers
  );
  const users = useUserStore((state) => state.users);
  const isLoading = useUserStore((state) => state.loading);
  const changesUsers = useUserStore((state) => state.changesUsers);
  const currentPage = useUserStore((state) => state.currentPage);
  const searchTerm = useUserStore((state) => state.searchTerm);

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
  useEffect(() => {
    if (!users.length) {
      fetchData();
    } else {
      getFilteredUsers(searchTerm);
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
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
          onClearSearch={() => {
            setSearchTerm("");
            getFilteredUsers("");
          }}
        />
      )}

      {/* Botón de Agregar */}
      <div className="flex justify-end mb-4 mt-4 gap-2">
        {/* Botón de crear */}
        <AddUserButton onClick={() => setIscreating(false)} />

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
                linkToEditOrCreate="/users/edit-create"
                user={user}
                onInactiveActive={onInactiveActive}
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
