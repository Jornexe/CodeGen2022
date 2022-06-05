fileinn = open(".netOutput.txt", mode="r", encoding="utf-8")
strings = fileinn.read().split(" ")
fileinn.close()
# print(str(strings))

fileout = open("From.netToPython.txt", mode="w", encoding="utf-8")
fileout.write(str(strings))
fileout.close()