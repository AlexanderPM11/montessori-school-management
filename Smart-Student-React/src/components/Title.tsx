import { FC, ReactNode } from "react";

interface TitleProps {
  children: ReactNode;
  className?: string;
}

export const Title: FC<TitleProps> = ({ children, className }) => {
  return (
    <h1 className={`text-xl md:text-2xl font-bold mb-4 sm:mb-6 ${className}`}>
      {children}
    </h1>
  );
};
