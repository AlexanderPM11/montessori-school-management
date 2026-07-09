import { FC, useRef, useEffect } from "react";

export interface FloatingMenuOption {
  label: string;
  icon?: React.ReactNode;
  onClick: () => void;
}

interface FloatingMenuProps {
  options: FloatingMenuOption[];
  isOpen: boolean;
  onClose: () => void;
  className?: string;
  style?: React.CSSProperties;
}

export const FloatingMenu: FC<FloatingMenuProps> = ({
  options,
  isOpen,
  onClose,
  className = "",
  style,
}) => {
  const menuRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        menuRef.current &&
        !menuRef.current.contains(event.target as Node) &&
        isOpen
      ) {
        setTimeout(() => {
          onClose();
        }, 100);
      }
    };

    if (isOpen) {
      document.addEventListener("mousedown", handleClickOutside);
    }

    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [isOpen, onClose]);

  if (!isOpen) return null;

  return (
    <div
      ref={menuRef}
      className={`absolute right-0 bottom-full mb-2 w-44 bg-white border border-gray-200 rounded-[5px] shadow-xl z-[1000] ${className}`}
      style={style}
    >
      <ul className="py-2">
        {options.map(({ label, icon, onClick }, index) => (
          <li key={index}>
            <button
              type="button"
              onClick={() => {
                onClick();
                // onClose();
              }}
              className="flex items-center gap-3 w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 transition-colors rounded-md bg-white"
            >
              {icon && <span className="text-lg text-gray-500">{icon}</span>}
              <span>{label}</span>
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
};
