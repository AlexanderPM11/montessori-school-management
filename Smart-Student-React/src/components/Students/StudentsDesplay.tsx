import { useEffect, useState } from "react";
import { useGeneralStore } from "../../Formik/Users/user";
import { roles } from "../../util/GeneralConst";
import { SearchBar } from "../Users/SearchBar";
import { Link } from "react-router-dom";
import { FaPlus } from "react-icons/fa";
import { UploadFile } from "../UploadFile";
import { IoCloudUploadOutline } from "react-icons/io5";
import { CustomPagination } from "../CustomPagination";
import { useStudentStore } from "../../hooks/store/Student/Students.store";
import { SearchStudent } from "../../util/Student/SearchStudent";
import { ItemStudent } from "./ItemStudent";
import { handleGeneralFunctionStudent } from "../../util/Student/HandleGeneralFunctionStudent";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";

interface Props {
  isParentsView?: boolean;
  idParent?: string;
}

export const StudentsDesplay = ({ idParent, isParentsView }: Props) => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [showExcelModal, setShowExcelModal] = useState(false);

  // Funciones del store
  const getAllUsersAsync = useStudentStore((state) => state.getAllUsersAsync);
  const onImportUsers = useStudentStore((state) => state.onImportUsers);
  const setCurrentDisplayUsers = useStudentStore(
    (state) => state.setcurrentDisplayStudents
  );
  const setChangesUsers = useStudentStore((state) => state.setChangesUsers);
  const setPageCount = useStudentStore((state) => state.setPageCount);
  const setSearchTerm = useStudentStore((state) => state.setSearchTerm);
  const setIscreating = useGeneralStore((state) => state.setIscreating);

  // Variables del store
  const currentDisplayUsers = useStudentStore(
    (state) => state.currentDisplayStudents
  );
  const students = useStudentStore((state) => state.students);
  const isLoading = useStudentStore((state) => state.loading);
  const getStudent = useStudentStore((state) => state.getStudent);
  const changesUsers = useStudentStore((state) => state.changesStudents);
  const currentPage = useStudentStore((state) => state.currentPage);
  const searchTerm = useStudentStore((state) => state.searchTerm);

  const getFilteredStudents = (value: string) => {
    const filteredUsers = SearchStudent({ students, searchTerm: value });

    const currentPageIndex = currentPage;
    const currentPageStart = currentPageIndex * 10;
    const currentPageEnd = (currentPageIndex + 1) * 10;

    if (value.trim() === "") {
      setPageCount(Math.ceil(students.length / 10));
      setCurrentDisplayUsers(
        students.slice(currentPageStart, currentPageEnd),
        currentPage
      );
      setChangesUsers(students);
    } else {
      setPageCount(Math.ceil(filteredUsers.length / 10));
      setCurrentDisplayUsers(
        filteredUsers.slice(currentPageStart, currentPageEnd),
        currentPage
      );
      setChangesUsers(filteredUsers);
    }
  };
  // const showBtnExcel = ManageLocalStorage.readFromLocalStorage<boolean>(
  //   "uploadUserExcelStudent"
  // );
  const showBtnExcel = true; // Cambiar a true para mostrar el botón de Excel
  const fetchData = async (loadForce: boolean) => {
    try {
      await getAllUsersAsync(loadForce, idParent, isParentsView);
    } catch (error) {
      console.error("Error fetching students:", error);
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

  useEffect(() => {
    fetchData(getStudent).then(() => {
      if (students.length && !isParentsView && !getStudent) {
        getFilteredStudents(searchTerm);
      }
    });

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [JSON.stringify(roles), idParent, isParentsView]);

  const displayContennt = !isLoading && students && students.length > 0;

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
            getFilteredStudents(value);
          }}
        />
      )}

      {/* Botón de Agregar */}
      {!isParentsView && (
        <div className="flex justify-end mb-4 mt-4 gap-2">
          {/* Botón de crear */}
          <Link
            className="bg-gray-900 border border-gray-900 h-10 w-10 text-white rounded-full 
             flex items-center justify-center text-[14px] 
             transition-all duration-200 ease-in-out 
             hover:bg-white hover:text-gray-900"
            to={"/student/edit-create-student"}
            onClick={() => setIscreating(false)}
            title="Crear"
          >
            <FaPlus />
          </Link>

          {showBtnExcel && (
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
          )}
        </div>
      )}

      <div
        className={`flex flex-wrap justify-start gap-4 items-start ${
          !isParentsView ? "" : "mt-6"
        }`}
      >
        {displayContennt &&
          currentDisplayUsers.map((student) => (
            <div key={student.id} className="w-full md:w-[calc(50%-0.5rem)]">
              <ItemStudent
                student={student}
                linkToEditOrCreate="/student/edit-create-student"
              />
            </div>
          ))}
      </div>

      {students && currentDisplayUsers.length <= 0 && (
        <div className="flex justify-center mt-5">
          <p className="font-semibold">Estudiante no encontrado</p>
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
