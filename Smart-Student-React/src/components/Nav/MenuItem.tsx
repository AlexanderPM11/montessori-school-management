import { NavLink } from "react-router-dom";

export const MenuItem = ({
  path,
  name,
  icon,
  badge,
  onClick,
}: {
  path: string;
  name: string;
  icon: React.ReactNode;
  badge?: string;
  onClick?: () => void;
}) => {
  return (
    <li>
      <NavLink
        to={path}
        onClick={onClick}
        className={({ isActive }) =>
          `flex w-[230px] items-center px-3 py-3 text-sm rounded-md transition-colors ${
            isActive
              ? "bg-sky-100 text-sky-800 border-l-4 border-sky-500"
              : "text-sky-300 hover:bg-sky-100 hover:text-sky-800"
          }`
        }
      >
        <span className="mr-3">{icon}</span>
        <span className="flex-1">{name}</span>
        {badge && (
          <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-amber-100 text-amber-800">
            {badge}
          </span>
        )}
      </NavLink>
    </li>
  );
};
