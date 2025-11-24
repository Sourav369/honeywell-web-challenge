// site.js – handles UI behaviour for upload, catalogue and video playback

document.addEventListener("DOMContentLoaded", () => {
  const root = document.getElementById("pageRoot");
  if (!root) {
    // Not on the Home/Index page – nothing to do.
    return;
  }

  const uploadView = document.getElementById("uploadView");
  const catalogueView = document.getElementById("catalogueView");
  const btnShowUpload = document.getElementById("btnShowUpload");
  const btnShowCatalogue = document.getElementById("btnShowCatalogue");

  const uploadForm = document.getElementById("uploadForm");
  const fileInput = document.getElementById("fileInput");
  const uploadMessage = document.getElementById("uploadMessage");
  const uploadSpinner = document.getElementById("uploadSpinner");
  const globalMessage = document.getElementById("globalMessage");

  const videoPlayer = document.getElementById("videoPlayer");
  const videoSource = document.getElementById("videoSource");
  const currentVideoLabel = document.getElementById("currentVideoLabel");
  const videoRows = document.querySelectorAll(".video-row");
  const playerColumn = document.getElementById("playerColumn"); // used to hide/show player

  const ACTIVE_CLASS = "btn-primary";
  const INACTIVE_CLASS = "btn-outline-primary";

  function setView(view) {
    if (!uploadView || !catalogueView) return;

    if (view === "upload") {
      uploadView.classList.remove("d-none");
      catalogueView.classList.add("d-none");

      // hide video player while in upload mode
      if (playerColumn) {
        playerColumn.classList.add("d-none");
      }

      btnShowUpload.classList.add(ACTIVE_CLASS);
      btnShowUpload.classList.remove(INACTIVE_CLASS);
      btnShowCatalogue.classList.add(INACTIVE_CLASS);
      btnShowCatalogue.classList.remove(ACTIVE_CLASS);
    } else {
      // catalogue
      catalogueView.classList.remove("d-none");
      uploadView.classList.add("d-none");

      // show video player in catalogue mode
      if (playerColumn) {
        playerColumn.classList.remove("d-none");
      }

      btnShowCatalogue.classList.add(ACTIVE_CLASS);
      btnShowCatalogue.classList.remove(INACTIVE_CLASS);
      btnShowUpload.classList.add(INACTIVE_CLASS);
      btnShowUpload.classList.remove(ACTIVE_CLASS);
    }
  }

  function showGlobalMessage(message, type) {
    if (!globalMessage) return;
    globalMessage.textContent = message;
    globalMessage.classList.remove(
      "d-none",
      "alert-success",
      "alert-danger",
      "alert-warning"
    );
    globalMessage.classList.add("alert", `alert-${type}`);
  }

  function clearGlobalMessage() {
    if (!globalMessage) return;
    globalMessage.textContent = "";
    globalMessage.classList.add("d-none");
  }

  // Initial view – default is catalogue
  const initialView = root.dataset.activeView || "catalogue";
  setView(initialView);

  // Toggle buttons
  btnShowUpload?.addEventListener("click", () => {
    clearGlobalMessage();
    setView("upload");
  });

  btnShowCatalogue?.addEventListener("click", () => {
    clearGlobalMessage();
    setView("catalogue");
  });

  // Upload handler
  if (uploadForm && fileInput) {
    uploadForm.addEventListener("submit", async (event) => {
      event.preventDefault();
      clearGlobalMessage();
      uploadMessage.textContent = "";

      const files = fileInput.files;
      if (!files || files.length === 0) {
        uploadMessage.textContent = "Please select at least one MP4 file.";
        return;
      }

      // Front-end validation for MP4
      for (const file of files) {
        const name = file.name.toLowerCase();
        if (!name.endsWith(".mp4")) {
          uploadMessage.textContent = `Only MP4 files are allowed. '${file.name}' is not an MP4 file.`;
          return;
        }
      }

      const formData = new FormData();
      for (const file of files) {
        formData.append("files", file);
      }

      uploadSpinner.classList.remove("d-none");
      try {
        const response = await fetch("/api/upload", {
          method: "POST",
          body: formData,
        });

        if (response.ok) {
          // Success – go to catalogue view with fresh data
          window.location.href = "/";
        } else {
          let message = "Upload failed. Please try again.";
          try {
            const data = await response.json();
            if (data && data.message) {
              message = data.message;
            }
          } catch {
            // ignore JSON parse errors
          }
          uploadMessage.textContent = message;
        }
      } catch (error) {
        console.error(error);
        uploadMessage.textContent =
          "An unexpected error occurred while uploading.";
      } finally {
        uploadSpinner.classList.add("d-none");
      }
    });
  }

  // Video row click handler – play selected video
  if (videoPlayer && videoSource && videoRows.length > 0) {
    videoRows.forEach((row) => {
      row.addEventListener("click", () => {
        const url = row.getAttribute("data-url");
        if (!url) return;

        // Visually highlight the selected row
        document
          .querySelectorAll(".video-row.active")
          .forEach((r) => r.classList.remove("active"));
        row.classList.add("active");

        // Update video source and play
        videoSource.setAttribute("src", url);
        videoPlayer.load();
        videoPlayer.play().catch((err) => console.error(err));

        if (currentVideoLabel) {
          const fileName = row.children[0]?.textContent ?? url;
          currentVideoLabel.textContent = `Now playing: ${fileName}`;
        }
      });
    });
  }
});
