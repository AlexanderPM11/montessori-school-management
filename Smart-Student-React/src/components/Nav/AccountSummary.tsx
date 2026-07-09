
import { UserSettings } from "./UserSettings"; // adjust import as needed

interface AccountSummaryProps {
  userName: string;
  email: string;
  avatarUrl?: string;
  onEditProfile: () => void;
  onLogout: () => void;
  className?: string; // Nueva prop
}

export const AccountSummary = ({
  userName,
  email,
  avatarUrl,
  onEditProfile,
  onLogout,
  className,
}: AccountSummaryProps) => {
  const defaultClass =
    "fixed bottom-3 flex items-center pb-10 md:pb-4 pl-6 md:pl-1 pt-4 max-w-[250px] w-full";

  return (
    <div className={className ? className : defaultClass}>
      <div className="flex items-center space-x-2">
        <img
          className="h-[50px] w-[50px] rounded-full object-cover"
          src={avatarUrl || "/default-avatar.png"}
          alt="User avatar"
        />
        <div className="flex flex-col items-start">
          <p
            className="cursor-pointer text-sm font-medium text-white"
            onClick={onEditProfile}
          >
            {userName}
          </p>
          <p
            className="cursor-pointer text-xs text-gray-300"
            onClick={onEditProfile}
          >
            {email}
          </p>
        </div>
      </div>
      <UserSettings onClickEditUser={onEditProfile} onLogOut={onLogout} />
    </div>
  );
};
