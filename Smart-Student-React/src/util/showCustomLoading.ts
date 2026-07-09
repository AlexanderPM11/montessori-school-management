import Swal from "sweetalert2";

export const showCustomLoading = (message: string = "") => {
    Swal.fire({
        title: message,
        html: `
      <div style="display: flex; justify-content: center; align-items: center;overflow: hidden;">
        <div class="spinner"></div>
      </div>
    `,
        showConfirmButton: false,
        allowOutsideClick: false,
        allowEscapeKey: false,
        didOpen: () => {
            const style = document.createElement("style");
            style.innerHTML = `
        .spinner {
          border: 4px solid rgba(0, 0, 0, 0.1);
          width: 50px;
          height: 50px;
          border-radius: 50%;
          border-left-color: #1f2937; /* rgb(31 41 55) */
          animation: spin 1s linear infinite;
        }

        @keyframes spin {
          to { transform: rotate(360deg); }
        }
      `;
            document.head.appendChild(style);
        },
    });
};

export const closeCustomLoading = () => {
    Swal.close();
};
