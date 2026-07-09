import { useMediaQuery } from "react-responsive";
import { useCallback, useEffect } from "react";

import { StudentsDesplay } from "./StudentsDesplay";
import { useStudentStore } from "../../hooks/store/Student/Students.store";
import { DesktopStudentTable } from "./DesktopStudentTable";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";
import { useGeneralStore } from "../../Formik/Users/user";
interface Props {
  isParentsView?: boolean;
  idParent?: string;
}
export const GeneralStudents = ({ idParent, isParentsView }: Props) => {
  const isDesktop = useMediaQuery({ minWidth: 1024 });

  // Funciones del store

  // Funciones del store
  const getAllUsersAsync = useStudentStore((state) => state.getAllUsersAsync);
  const onInactiveActive = useStudentStore((state) => state.onInactiveActive);
  // Variables del store
  const currentDisplayUsers = useStudentStore(
    (state) => state.imutableStudents
  );
  const isLoading = useStudentStore((state) => state.loading);
  const onGetData = useGeneralStore((state) => state.onGetData);

  const fetchData = async (loadForce: boolean) => {
    try {
      await onGetData();
      await getAllUsersAsync(loadForce, idParent, isParentsView);
    } catch (error) {
      console.error("Error fetching students:", error);
    }
  };
  const onToggleStatus = useCallback(
    (id: number, estado: string) => {
      return onInactiveActive(id, estado);
    },
    [onInactiveActive]
  );
  useEffect(() => {
    const forceLoad = isParentsView ? false : currentDisplayUsers.length < 7;
    fetchData(forceLoad);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  if (isLoading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }
  return isDesktop ? (
    <DesktopStudentTable
      users={currentDisplayUsers}
      onToggleStatus={onToggleStatus}
      isParentsView={isParentsView}
    />
  ) : (
    <StudentsDesplay idParent={idParent} isParentsView={isParentsView} />
  );
};
