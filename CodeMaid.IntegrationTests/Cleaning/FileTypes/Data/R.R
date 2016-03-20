"This function takes a value x, and does things and returns things that
 take several lines to explain"
doEverythingOften <- function(x) {
    # Non! Comment it out! We'll just do it once for now.
    "if (x %in% 1:9) {
          doTenEverythings()
     }"
    doEverythingOnce()

    return(list(
         everythingDone = TRUE,
         howOftenDone = 1))
}