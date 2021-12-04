import sys
import tkinter as tk
import tkinter.filedialog as fd

#   Desc: Multi select file panel
#         - called by c# script
#   Date: 2021/12/4
#   Author: dont-laugh

TK_SILENCE_DEPRECATION = 1  # Suppress warning

TITLE = '-title'
FILETYPES = '-filetypes'
DIRECTORY = '-directory'

def get_file_types(s):
    types = s.split(';')
    f_types = []
    for pair in types:
        kv = pair.split(',')
        f_types.append((kv[0], kv[1]))
    return f_types


if __name__ == '__main__':
    root = tk.Tk()
    root.withdraw()

    title = ''
    file_types = [('All Files', '*')]
    directory = ''

    for i in range(1, len(sys.argv), 2):
        arg_name = sys.argv[i]
        arg_val = sys.argv[i + 1]
        if arg_name == TITLE:
            title = arg_val
        elif arg_name == FILETYPES:
            file_types = get_file_types(arg_val)
        elif arg_name == DIRECTORY:
            directory = arg_val

    files = fd.askopenfilenames(parent=root, title=title, filetypes=file_types, initialdir=directory)
    print(files)
