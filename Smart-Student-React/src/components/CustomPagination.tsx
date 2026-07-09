import { GrNext } from "react-icons/gr";
import { MdArrowBackIos } from "react-icons/md";
import ReactPaginate from "react-paginate";

interface Props<T> {
  currentPage: number;
  pageCount: number;
  itemDisplayCount: number;
  items: T[];
  onPageChange: (itemsToDisplay: T[], currentPage: number) => void;
}

export const CustomPagination = <T,>({
  items,
  itemDisplayCount,
  pageCount,
  onPageChange,
  currentPage,
}: Props<T>) => {
  // const pageCount = Math.ceil(items.length / itemDisplayCount);

  const handlePageChange = (selectedPage: { selected: number }) => {
    // Calcular el índice de inicio y fin para los elementos de la página seleccionada
    const startIndex = selectedPage.selected * itemDisplayCount;
    const endIndex = startIndex + itemDisplayCount;
    // Filtrar los items que corresponden a la página seleccionada
    const itemsToDisplay = items.slice(startIndex, endIndex);

    // Llamar a la función onPageChange con los elementos filtrados
    onPageChange(itemsToDisplay, selectedPage.selected);
  };

  return (
    <div className="flex justify-center items-center max-w-[500px] mx-auto">
      <ReactPaginate
        breakLabel="..."
        nextLabel={<GrNext />}
        previousLabel={<MdArrowBackIos />}
        onPageChange={handlePageChange} // Usar el handler personalizado
        pageRangeDisplayed={2}
        pageCount={pageCount}
        marginPagesDisplayed={3}
        containerClassName="flex gap-2 items-center justify-center"
        pageClassName="pagination-item"
        pageLinkClassName=" md:text-[20px]"
        activeClassName="bg-gray-900 bg-blue-600 text-white h-[40px] md:h-[50px] md:w-[50px] w-[40px] md:text-[25px] rounded-full flex items-center justify-center"
        disabledClassName=""
        forcePage={currentPage}
        renderOnZeroPageCount={null}
      />
    </div>
  );
};
