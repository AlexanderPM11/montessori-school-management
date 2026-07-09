import { useEffect } from "react";
import { SearchBar } from "../Users/SearchBar";
import { Link } from "react-router-dom";
import { FaPlus } from "react-icons/fa";
import { CustomPagination } from "../CustomPagination";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";
import { ItemSubject } from "./ItemSubject";
import { useSubjectStore } from "../../hooks/store/Subject/Subject.store";
import { SearchSubject } from "../../util/Subject/SearchSubject";

export const SubjectsDesplay = () => {
  // Funciones del store
  const getSubject = useSubjectStore((state) => state.getSubject);
  const setcurrentDisplaySubjects = useSubjectStore(
    (state) => state.setcurrentDisplaySubjects
  );
  const setchangesSubjects = useSubjectStore(
    (state) => state.setChangesSubjects
  );
  const setPageCount = useSubjectStore((state) => state.setPageCount);
  const setSearchTerm = useSubjectStore((state) => state.setSearchTerm);

  // Variables del store
  const currentDisplaySubjects = useSubjectStore(
    (state) => state.currentDisplaySubjects
  );
  const subjects = useSubjectStore((state) => state.subjects);
  const isLoading = useSubjectStore((state) => state.loading);
  const changesSubjects = useSubjectStore((state) => state.changesSubjects);
  const currentPage = useSubjectStore((state) => state.currentPage);
  const searchTerm = useSubjectStore((state) => state.searchTerm);

  const getFilteredsubjects = (value: string) => {
    const filteredUsers = SearchSubject({ subjects, searchTerm: value });

    const currentPageIndex = currentPage;
    const currentPageStart = currentPageIndex * 10;
    const currentPageEnd = (currentPageIndex + 1) * 10;

    if (value.trim() === "") {
      setPageCount(Math.ceil(subjects.length / 10));
      setcurrentDisplaySubjects(
        subjects.slice(currentPageStart, currentPageEnd),
        currentPage
      );
      setchangesSubjects(subjects);
    } else {
      setPageCount(Math.ceil(filteredUsers.length / 10));
      setcurrentDisplaySubjects(
        filteredUsers.slice(currentPageStart, currentPageEnd),
        currentPage
      );
      setchangesSubjects(filteredUsers);
    }
  };

  const fetchData = async () => {
    try {
      await getSubject();
    } catch (error) {
      console.error("Error fetching subjects:", error);
    }
  };

  useEffect(() => {
    fetchData().then(() => {
      if (subjects.length) {
        getFilteredsubjects(searchTerm);
      }
    });

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const displayContennt = !isLoading && subjects && subjects.length > 0;

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
            getFilteredsubjects(value);
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
          to={"/subjects/edit-create"}
          title="Crear"
        >
          <FaPlus />
        </Link>
      </div>

      <div className="flex flex-wrap justify-start gap-4 items-start mt-6">
        {displayContennt &&
          currentDisplaySubjects.map((subject) => (
            <div key={subject.id} className="w-full md:w-[calc(50%-0.5rem)]">
              <ItemSubject
                subject={subject}
                linkToEditOrCreate="/subjects/edit-create"
              />
            </div>
          ))}
      </div>

      {subjects && currentDisplaySubjects.length <= 0 && (
        <div className="flex justify-center mt-5">
          <p className="font-semibold">Asignatura no encontrada</p>
        </div>
      )}

      {displayContennt && (
        <div className="flex justify-center mt-5 mb-5">
          <CustomPagination
            itemDisplayCount={10}
            items={changesSubjects}
            pageCount={changesSubjects.length / 10}
            currentPage={currentPage}
            onPageChange={(value, page) => {
              setcurrentDisplaySubjects(value, page);
            }}
          />
        </div>
      )}
    </div>
  );
};
