import { Outlet } from "react-router-dom";
import { DesktopNav } from "./Nav/DesktopNav";
import { MobileNav } from "./Nav/MobileNav";
import { useState } from "react";
import { TopBar } from "./Nav/TopBar";

export const NavBar = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const toggleMenu = () => setIsMenuOpen(!isMenuOpen);

  return (
    <div className="h-screen">
      <MobileNav isMenuOpen={isMenuOpen} toggleMenu={toggleMenu} />
      <DesktopNav toggleMenu={toggleMenu} />

      {/* TopBar añadido */}
      <TopBar />

      <div className="w-full">
        <div className="lg:ml-[313px] transform ease-in-out transition duration-500 p-4 pt-20">
          <Outlet />
        </div>
      </div>
    </div>
  );
};
