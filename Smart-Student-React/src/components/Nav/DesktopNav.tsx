import { Logo } from "../Logo";
import { AccountSummary } from "./AccountSummary";
import { useAuthStore } from "../../hooks/store/Auth.store";
import { appMenuItems } from "./MenuItems";
import { MenuSection } from "./MenuSection";
import { MenuItem } from "./MenuItem";
import { getItemDB } from "../../util/indexedDB";
import { useEffect, useState } from "react";

export const DesktopNav = ({ toggleMenu }: { toggleMenu: () => void }) => {
  const logOut = useAuthStore((state) => state.logout);
  const userLoggued = useAuthStore((state) => state.UserLoggued);
  const [imageUrl, setImageUrl] = useState<string | null>(null);

  // Aquí obtienes los roles actuales del usuario
  const userRoles = userLoggued?.roles || []; // Asegúrate de inicializar como array vacío si no hay

  // Función para filtrar ítems permitidos
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
    async function loadImage() {
      const url = await getItemDB<string>("user-image");
      setImageUrl(url);
    }
    loadImage();
  }, []);
  return (
    <div className="hidden rounded-br-[40px] lg:flex flex-col bg-gray-900 h-screen fixed border-r border-gray-700 w-[300px] z-[1000]">
      {/* Logo Section */}
      <div className="p-6 flex items-center justify-between">
        <Logo />
      </div>

      {/* Main Navigation */}
      <nav className="flex-1 overflow-y-auto px-4 py-2 space-y-1">
        {mainItems.length > 0 && (
          <MenuSection title="Principal">
            {mainItems.map((item) => (
              <MenuItem
                key={item.path}
                path={item.path}
                name={item.name}
                icon={item.icon}
              />
            ))}
          </MenuSection>
        )}

        {userItems.length > 0 && (
          <MenuSection title="Usuarios">
            {userItems.map((item) => (
              <MenuItem
                key={item.path}
                path={item.path}
                name={item.name}
                icon={item.icon}
              />
            ))}
          </MenuSection>
        )}
        <div className="pb-4"></div>

        {academicItems.length > 0 && (
          <MenuSection title="Academia">
            {academicItems.map((item) => (
              <MenuItem
                key={item.path}
                path={item.path}
                name={item.name}
                icon={item.icon}
              />
            ))}
          </MenuSection>
        )}

        <div className="pb-4"></div>
      </nav>

      {/* User Account Section */}
      <div className="p-4 border-t border-gray-800  h-[100px]">
        <AccountSummary
          userName={userLoggued.userName}
          email={userLoggued.email}
          avatarUrl={imageUrl ?? ""}
          onEditProfile={toggleMenu}
          onLogout={logOut}
        />
      </div>
    </div>
  );
};
