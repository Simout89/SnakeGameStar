mergeInto(LibraryManager.library, {
  LS_Set: function (keyPtr, valuePtr) {
    const key = UTF8ToString(keyPtr);
    const value = UTF8ToString(valuePtr);
    localStorage.setItem(key, value);
  },

  LS_Get: function (keyPtr) {
    const key = UTF8ToString(keyPtr);
    const value = localStorage.getItem(key);
    if (value === null) return 0;

    const buffer = lengthBytesUTF8(value) + 1;
    const ptr = _malloc(buffer);
    stringToUTF8(value, ptr, buffer);
    return ptr;
  },

  LS_Delete: function (keyPtr) {
    const key = UTF8ToString(keyPtr);
    localStorage.removeItem(key);
  }
});
