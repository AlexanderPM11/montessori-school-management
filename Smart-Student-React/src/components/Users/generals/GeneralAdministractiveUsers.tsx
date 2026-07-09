import { useMediaQuery } from "react-responsive";
import { AdministractiveUsers } from "./AdministractiveUsers";
import { useUserStore } from "../../../hooks/store/Users/Users.store";
import { useEffect, useMemo, useCallback } from "react";
import { roles } from "../../../util/GeneralConst";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../util/showCustomLoading";
import { DesktopUserTable } from "./DesktopTableAdmiUsers/DesktopUserTable";

export const GeneralAdministractiveUsers = () => {
  const isDesktop = useMediaQuery({ minWidth: 1024 });

  const getAllUsersAsync = useUserStore((state) => state.getAllUsersAsync);
  const users = useUserStore((state) => state.users);
  const currentDisplayUsers = useUserStore((state) => state.imutableUsers);
  const setCurrentDisplayUsers = useUserStore(
    (state) => state.setCurrentDisplayUsers
  );
  const setPageCount = useUserStore((state) => state.setPageCount);
  const setChangesUsers = useUserStore((state) => state.setChangesUsers);
  const currentPage = useUserStore((state) => state.currentPage);
  const isloading = useUserStore((state) => state.loading);
  const onInactiveActive = useUserStore((state) => state.onInactiveActive);

  useEffect(() => {
    if (users.length === undefined) {
      getAllUsersAsync([
        roles.admin,
        roles.coordinator,
        roles.teacher,
        roles.rector,
        roles.secretary,
        roles.basic,
      ]);
    } else {
      const start = currentPage * 10;
      const end = start + 10;
      setCurrentDisplayUsers(users.slice(start, end), currentPage);
      setPageCount(Math.ceil(users.length / 10));
      setChangesUsers(users);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [users, currentPage]);

  if (isloading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }

  // Memoizar currentDisplayUsers para evitar pasar referencia nueva si no cambian datos
  const memoizedUsers = useMemo(
    () => currentDisplayUsers,
    [currentDisplayUsers]
  );

  // Memoizar onToggleStatus para que no cambie en cada render
  const onToggleStatus = useCallback(
    (id: string) => {
      return onInactiveActive(id);
    },
    [onInactiveActive]
  );

  return (
    <div className="">
      {isDesktop ? (
        <DesktopUserTable
          users={memoizedUsers}
          onToggleStatus={onToggleStatus}
        />
      ) : (
        <AdministractiveUsers />
      )}
    </div>
  );
};
