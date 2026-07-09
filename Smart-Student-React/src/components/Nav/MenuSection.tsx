export const MenuSection = ({
  title,
  children,
}: {
  title: string;
  children: React.ReactNode;
}) => {
  return (
    <div className="mb-6">
      <h3 className="text-xs font-semibold text-gray-400 uppercase tracking-wider px-3 mb-2">
        {title}
      </h3>
      <ul className="space-y-1">{children}</ul>
    </div>
  );
};
