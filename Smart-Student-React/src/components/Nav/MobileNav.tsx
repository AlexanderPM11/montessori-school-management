import { useEffect, useRef, useState } from "react";
import { Logo } from "../Logo";
import { MenuItem } from "./MenuItem";
import { FaBarsStaggered } from "react-icons/fa6";
import { useAuthStore } from "../../hooks/store/Auth.store";
import { AccountSummary } from "./AccountSummary";
import { appMenuItems } from "./MenuItems";
import { MenuSection } from "./MenuSection";
import { getItemDB } from "../../util/indexedDB";
import { FiBell } from "react-icons/fi";
import { DialogCusmtom } from "../../util/DialogCusmtom";

export const MobileNav = ({
  isMenuOpen,
  toggleMenu,
}: {
  isMenuOpen: boolean;
  toggleMenu: () => void;
}) => {
  const logOut = useAuthStore((state) => state.logout);
  const userLoggued = useAuthStore((state) => state.UserLoggued);
  const [imageUrl, setImageUrl] = useState<string | null>(null);

  const menuRef = useRef<HTMLDivElement>(null);

  const userRoles = userLoggued?.roles || [];

  const filterItemsByRole = (section: string) => {
    return appMenuItems
      .filter((item) => item.section === section)
      .filter((item) =>
        userRoles.some((role: string) => item.allowedRoles.includes(role))
      );
  };

  const mainItems = filterItemsByRole("main");
  const userItems = filterItemsByRole("users");
  const academicItems = filterItemsByRole("academic");

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent | TouchEvent) => {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        toggleMenu();
      }
    };

    if (isMenuOpen) {
      document.addEventListener("mousedown", handleClickOutside);
      document.addEventListener("touchstart", handleClickOutside);
    }

    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
      document.removeEventListener("touchstart", handleClickOutside);
    };
  }, [isMenuOpen, toggleMenu]);

  useEffect(() => {
    async function loadImage() {
      const url = await getItemDB<string>("user-image");
      setImageUrl(url);
    }
    loadImage();
  }, []);
  const [isDialogOpen, setIsDialogOpen] = useState(false);

  return (
    <div className="lg:hidden">
      {/* Topbar */}
      <div className="fixed top-0 left-0 w-full bg-gray-900 h-[70px] flex items-center justify-between pl-2 pr-2 text-white z-50">
        <Logo />
        <div className="flex justify-end rounded-lg w-full mr-[75px] ">
          <div className="relative">
            <button
              onClick={() => setIsDialogOpen(true)}
              className="px-3 py-1 rounded-lg flex items-center gap-1"
            >
              <FiBell className="w-5 h-5 text-white" />
            </button>

            {/* Badge de notificaciones */}
            <span className="absolute -top-1 -right-1 inline-flex items-center justify-center w-5 h-5 text-xs font-bold leading-none text-white bg-red-500 rounded-full">
              0
            </span>
          </div>
        </div>
        <div className="relative">
          <div className="absolute top-1/2 right-0 transform -translate-x-1/2 -translate-y-1/2 z-50">
            <button
              onClick={toggleMenu}
              className="text-[24px] font-semibold p-2 rounded-md"
            >
              {isMenuOpen ? <></> : <FaBarsStaggered />}
            </button>
          </div>
          <DialogCusmtom
            isOpen={isDialogOpen}
            onClose={() => setIsDialogOpen(false)}
            title="Notificaciones"
          >
            <p className="text-[14px] italic font-bold text-center text-gray-800">
              No hay notificaciones disponibles.
            </p>
          </DialogCusmtom>
        </div>
      </div>

      {/* Sidebar */}
      <div
        ref={menuRef}
        className={`w-[80%] max-w-[300px] rounded-br-[20px] fixed inset-0 transform ease-in-out transition duration-500 bg-gray-900 flex flex-col z-40 ${
          isMenuOpen ? "flex" : "hidden"
        } h-[100dvh]`}
      >
        <div className="xl:flex justify-start p-4 mb-2 items-center space-x-3">
          <Logo />
        </div>

        {/* Contenido scrollable */}
        <div className="flex-1 overflow-y-auto mt-2 flex flex-col justify-start items-start pl-4 w-full border-gray-600 border-b space-y-3 pb-10">
          {mainItems.length > 0 && (
            <MenuSection title="Principal">
              {mainItems.map((item) => (
                <div className="w-[250px]" key={item.path}>
                  <MenuItem
                    path={item.path}
                    name={item.name}
                    icon={item.icon}
                    onClick={toggleMenu}
                  />
                </div>
              ))}
            </MenuSection>
          )}

          {userItems.length > 0 && (
            <MenuSection title="Usuarios">
              {userItems.map((item) => (
                <div className="w-[250px]" key={item.path}>
                  <MenuItem
                    path={item.path}
                    name={item.name}
                    icon={item.icon}
                    onClick={toggleMenu}
                  />
                </div>
              ))}
            </MenuSection>
          )}
          <div className="pb-4"></div>
          {academicItems.length > 0 && (
            <MenuSection title="Academia">
              {academicItems.map((item) => (
                <div className="w-[250px]" key={item.path}>
                  <MenuItem
                    path={item.path}
                    name={item.name}
                    icon={item.icon}
                    onClick={toggleMenu}
                  />
                </div>
              ))}
            </MenuSection>
          )}
        </div>

        {/* Account siempre fijo al fondo */}
        <AccountSummary
          userName={userLoggued.userName}
          email={userLoggued.email}
          avatarUrl={imageUrl ?? ""}
          onEditProfile={toggleMenu}
          onLogout={logOut}
          className="relative w-full flex items-center p-4 justify-between mb-12 pb-[env(safe-area-inset-bottom)]"
        />
      </div>
    </div>
  );
};
