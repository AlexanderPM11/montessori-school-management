import { useMediaQuery } from "react-responsive";
import { useEffect } from "react";

import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";
import { useSubjectStore } from "../../hooks/store/Subject/Subject.store";
import { SubjectsDesplay } from "./SubjectsDesplay";
import { DesktopSubjectTable } from "./DesktopSubjectTable";

export const GeneralSubjects = () => {
  const isDesktop = useMediaQuery({ minWidth: 1024 });
  const onDelete = useSubjectStore((state) => state.onDelete);

  // Funciones del store
  const getSubject = useSubjectStore((state) => state.getSubject);
  const currentDisplaySubjects = useSubjectStore(
    (state) => state.currentDisplaySubjects
  );
  const isLoading = useSubjectStore((state) => state.loading);

  const fetchData = async () => {
    try {
      await getSubject();
    } catch (error) {
      console.error("Error fetching subjects:", error);
    }
  };

  useEffect(() => {
    fetchData();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  if (isLoading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }
  return isDesktop ? (
    <DesktopSubjectTable users={currentDisplaySubjects} onDelete={onDelete} />
  ) : (
    <SubjectsDesplay />
  );
};
