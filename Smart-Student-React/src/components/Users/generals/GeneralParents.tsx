import { useMediaQuery } from "react-responsive";
import { useEffect, useMemo, useCallback } from "react";
import { roles } from "../../../util/GeneralConst";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../util/showCustomLoading";
import { Parents } from "./Parents";
import { DesktopUserParentsTable } from "./DesktopTableParentsUsers/DesktopUserParentsTable";
import { useUserParentsStore } from "../../../Formik/Users/user";

export const GeneralParents = () => {
  const isDesktop = useMediaQuery({ minWidth: 1024 });

  // Funciones del store
  const getAllUsersAsync = useUserParentsStore(
    (state) => state.getAllUsersAsync
  );

  const onInactiveActive = useUserParentsStore(
    (state) => state.onInactiveActive
  );

  // Variables del store
  const currentDisplayUsers = useUserParentsStore(
    (state) => state.imutableUsers
  );
  const users = useUserParentsStore((state) => state.users);
  const isLoading = useUserParentsStore((state) => state.loading);

  const rolesParents = [roles.parents];

  const fetchData = async (loadForce?: boolean) => {
    try {
      await getAllUsersAsync(rolesParents, loadForce);
    } catch (error) {
      console.error("Error fetching users:", error);
    }
  };

  useEffect(() => {
    if (!users.length) {
      fetchData();
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [JSON.stringify(roles)]);

  if (isLoading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }

  const memoizedUsers = useMemo(
    () => currentDisplayUsers,
    [currentDisplayUsers]
  );

  const onToggleStatus = useCallback(
    (id: string) => {
      return onInactiveActive(id);
    },
    [onInactiveActive]
  );

  return isDesktop ? (
    <DesktopUserParentsTable
      users={memoizedUsers}
      onToggleStatus={onToggleStatus}
    />
  ) : (
    <Parents />
  );
};
