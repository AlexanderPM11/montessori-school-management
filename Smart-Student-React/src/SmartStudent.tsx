import { ToastContainer } from "react-toastify";
import { RouterProvider } from "react-router-dom";
import { ScrollToTopButton } from "./components/ScrollToTopButton";
import { router } from "./routes/router";
import { ScrollToBottomButton } from "./components/ScrollToBottomButton";

export const SmartStudent = () => {
  return (
    <>
      <RouterProvider router={router} />
      <ToastContainer />
      <ScrollToTopButton />
      <ScrollToBottomButton />
    </>
  );
};
